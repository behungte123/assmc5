using Lab4.Models;
using System.ComponentModel.DataAnnotations;

namespace Lab4.Models
{
    public class Cart
    {
        public int Id { get; set; }                       // PK

        // Nếu có đăng nhập: lưu theo UserId (tuỳ bạn)
        public string? UserId { get; set; }

        // Nếu chưa có đăng nhập: lưu theo SessionId (khuyên dùng cho lab)
        [MaxLength(100)]
        public string? SessionId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<CartItem> Items { get; set; } = new();
    }
    public class CartItem
    {
        public int Id { get; set; }            // PK

        public int CartId { get; set; }        // FK -> Carts
        public Cart Cart { get; set; } = null!;

        public int ProductId { get; set; }     // FK -> Products
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; } = 1;

        // Lưu giá tại thời điểm thêm (tuỳ bạn có cần không)
        [MaxLength(50)]
        public string UnitPriceText { get; set; } = "";
    }
}
