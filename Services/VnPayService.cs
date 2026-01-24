using Lab4.Models;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Lab4.Services
{
    /// <summary>
    /// VNPay service implementation
    /// </summary>
    public class VnPayService : IVnPayService
    {
        private readonly VnPayConfig _config;

        public VnPayService(IOptions<VnPayConfig> config)
        {
            _config = config.Value;
        }

        /// <summary>
        /// Tạo URL thanh toán VNPay
        /// </summary>
        /// <param name="bankCode">Mã ngân hàng (VNPAYQR để hiện QR, null để hiện danh sách ngân hàng)</param>
        public string CreatePaymentUrl(int orderId, decimal amount, string orderInfo, string ipAddress, string? bankCode = null)
        {
            var vnpParams = new SortedList<string, string>();

            // Các tham số bắt buộc
            vnpParams.Add("vnp_Version", _config.Version);
            vnpParams.Add("vnp_Command", "pay");
            vnpParams.Add("vnp_TmnCode", _config.TmnCode);
            vnpParams.Add("vnp_Amount", ((long)(amount * 100)).ToString()); // VNPay yêu cầu nhân 100
            vnpParams.Add("vnp_CurrCode", "VND");
            vnpParams.Add("vnp_TxnRef", orderId.ToString());
            vnpParams.Add("vnp_OrderInfo", orderInfo);
            vnpParams.Add("vnp_OrderType", "other");
            vnpParams.Add("vnp_Locale", "vn");
            vnpParams.Add("vnp_ReturnUrl", _config.ReturnUrl);
            vnpParams.Add("vnp_IpAddr", ipAddress);
            vnpParams.Add("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            
            // Thêm BankCode nếu có (VNPAYQR để hiện QR code)
            if (!string.IsNullOrEmpty(bankCode))
            {
                vnpParams.Add("vnp_BankCode", bankCode);
            }

            // Tạo query string
            var queryBuilder = new StringBuilder();
            foreach (var kv in vnpParams)
            {
                if (queryBuilder.Length > 0)
                    queryBuilder.Append('&');
                queryBuilder.Append(WebUtility.UrlEncode(kv.Key));
                queryBuilder.Append('=');
                queryBuilder.Append(WebUtility.UrlEncode(kv.Value));
            }

            // Tạo chữ ký HMACSHA512
            var signData = queryBuilder.ToString();
            var vnpSecureHash = HmacSHA512(_config.HashSecret, signData);

            // Thêm chữ ký vào URL
            queryBuilder.Append("&vnp_SecureHash=");
            queryBuilder.Append(vnpSecureHash);

            return $"{_config.BaseUrl}?{queryBuilder}";
        }

        /// <summary>
        /// Xác thực callback từ VNPay
        /// </summary>
        public VnPayCallbackResult ValidateCallback(IQueryCollection queryParams)
        {
            var result = new VnPayCallbackResult();

            try
            {
                // Lấy các giá trị từ query params
                var vnpSecureHash = queryParams["vnp_SecureHash"].ToString();
                var vnpResponseCode = queryParams["vnp_ResponseCode"].ToString();
                var vnpTxnRef = queryParams["vnp_TxnRef"].ToString();
                var vnpTransactionNo = queryParams["vnp_TransactionNo"].ToString();
                var vnpAmount = queryParams["vnp_Amount"].ToString();
                var vnpPayDate = queryParams["vnp_PayDate"].ToString();

                // Xây dựng lại chuỗi để verify
                var vnpParams = new SortedList<string, string>();
                foreach (var key in queryParams.Keys)
                {
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_") && 
                        key != "vnp_SecureHash" && key != "vnp_SecureHashType")
                    {
                        vnpParams.Add(key, queryParams[key].ToString());
                    }
                }

                var signData = new StringBuilder();
                foreach (var kv in vnpParams)
                {
                    if (signData.Length > 0)
                        signData.Append('&');
                    signData.Append(WebUtility.UrlEncode(kv.Key));
                    signData.Append('=');
                    signData.Append(WebUtility.UrlEncode(kv.Value));
                }

                var checkSign = HmacSHA512(_config.HashSecret, signData.ToString());

                // Verify chữ ký
                if (checkSign.Equals(vnpSecureHash, StringComparison.InvariantCultureIgnoreCase))
                {
                    result.IsValid = true;
                    result.OrderId = int.TryParse(vnpTxnRef, out var orderId) ? orderId : 0;
                    result.TransactionNo = vnpTransactionNo;
                    result.ResponseCode = vnpResponseCode;
                    result.IsSuccess = vnpResponseCode == "00";
                    result.Message = GetResponseMessage(vnpResponseCode);

                    // Parse amount (VNPay trả về amount * 100)
                    if (long.TryParse(vnpAmount, out var amountValue))
                    {
                        result.Amount = amountValue / 100m;
                    }

                    // Parse pay date
                    if (!string.IsNullOrEmpty(vnpPayDate) && 
                        DateTime.TryParseExact(vnpPayDate, "yyyyMMddHHmmss", 
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out var payDate))
                    {
                        result.PayDate = payDate;
                    }
                }
                else
                {
                    result.IsValid = false;
                    result.Message = "Chữ ký không hợp lệ";
                }
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Message = $"Lỗi xử lý callback: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// Tạo HMAC SHA512 hash
        /// </summary>
        private static string HmacSHA512(string key, string data)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Lấy message tương ứng với response code
        /// </summary>
        private static string GetResponseMessage(string responseCode)
        {
            return responseCode switch
            {
                "00" => "Giao dịch thành công",
                "07" => "Trừ tiền thành công. Giao dịch bị nghi ngờ (liên quan tới lừa đảo, giao dịch bất thường)",
                "09" => "Thẻ/Tài khoản của khách hàng chưa đăng ký dịch vụ InternetBanking tại ngân hàng",
                "10" => "Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần",
                "11" => "Đã hết hạn chờ thanh toán. Xin quý khách vui lòng thực hiện lại giao dịch",
                "12" => "Thẻ/Tài khoản của khách hàng bị khóa",
                "13" => "Quý khách nhập sai mật khẩu xác thực giao dịch (OTP)",
                "24" => "Khách hàng hủy giao dịch",
                "51" => "Tài khoản của quý khách không đủ số dư để thực hiện giao dịch",
                "65" => "Tài khoản của Quý khách đã vượt quá hạn mức giao dịch trong ngày",
                "75" => "Ngân hàng thanh toán đang bảo trì",
                "79" => "KH nhập sai mật khẩu thanh toán quá số lần quy định",
                "99" => "Lỗi không xác định",
                _ => $"Lỗi không xác định (Mã: {responseCode})"
            };
        }
    }
}
