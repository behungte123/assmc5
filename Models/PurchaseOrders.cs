using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace lab4.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string? Code { get; set; } = null!;
        public int SupplierId { get; set; }
        [ValidateNever]
        public Supplier Supplier { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; } = null!;

        public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }

}
