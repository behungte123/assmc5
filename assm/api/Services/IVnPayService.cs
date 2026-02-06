using Lab4.Models;

namespace Lab4.Services
{
    /// <summary>
    /// Interface cho VNPay service
    /// </summary>
    public interface IVnPayService
    {
        /// <summary>
        /// Tạo URL thanh toán VNPay
        /// </summary>
        /// <param name="orderId">Mã đơn hàng</param>
        /// <param name="amount">Số tiền (VND)</param>
        /// <param name="orderInfo">Mô tả đơn hàng</param>
        /// <param name="ipAddress">IP khách hàng</param>
        /// <returns>URL redirect đến VNPay</returns>
        string CreatePaymentUrl(int orderId, decimal amount, string orderInfo, string ipAddress, string? bankCode = null);

        /// <summary>
        /// Xác thực callback từ VNPay
        /// </summary>
        /// <param name="queryParams">Query parameters từ VNPay</param>
        /// <returns>Kết quả xác thực</returns>
        VnPayCallbackResult ValidateCallback(IQueryCollection queryParams);
    }

    /// <summary>
    /// Kết quả xác thực callback VNPay
    /// </summary>
    public class VnPayCallbackResult
    {
        public bool IsValid { get; set; }
        public bool IsSuccess { get; set; }
        public int OrderId { get; set; }
        public string TransactionNo { get; set; } = string.Empty;
        public string ResponseCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime? PayDate { get; set; }
    }
}
