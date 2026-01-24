namespace Lab4.Models
{
    /// <summary>
    /// Cấu hình VNPay từ appsettings.json
    /// </summary>
    public class VnPayConfig
    {
        /// <summary>
        /// Mã website tại VNPay (do VNPay cấp)
        /// </summary>
        public string TmnCode { get; set; } = string.Empty;

        /// <summary>
        /// Chuỗi bí mật để tạo checksum
        /// </summary>
        public string HashSecret { get; set; } = string.Empty;

        /// <summary>
        /// URL thanh toán VNPay
        /// Sandbox: https://sandbox.vnpayment.vn/paymentv2/vpcpay.html
        /// Production: https://pay.vnpay.vn/vpcpay.html
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// URL callback sau khi thanh toán
        /// </summary>
        public string ReturnUrl { get; set; } = string.Empty;

        /// <summary>
        /// Phiên bản API VNPay
        /// </summary>
        public string Version { get; set; } = "2.1.0";
    }
}
