using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4.Models
{
    public class Product
    {
        [Key] // ✅ Primary Key
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Description { get; set; }

        /// <summary>
        /// Giá bán (VND). Lưu số nguyên để tránh lỗi làm tròn.
        /// Ví dụ: 90000
        /// </summary>
        [Range(0, int.MaxValue)]
        public int PriceVnd { get; set; }

        /// <summary>
        /// Đường dẫn ảnh hiển thị (vd: /images/foods/bun-bo-hue.png)
        /// </summary>
        [StringLength(260)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Thứ tự hiển thị trên trang Menu/Home (nhỏ lên trước)
        /// </summary>
        public int SortOrder { get; set; } = 0;

        [NotMapped]
        public string PriceText => $"{PriceVnd:N0} VND";
    }
}
