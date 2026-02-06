namespace lab4.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        // nghiệp vụ
        public int Rating { get; set; }          // 1–5
        public decimal? DefaultDiscount { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    }


}
