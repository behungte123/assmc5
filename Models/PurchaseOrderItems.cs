using Lab4.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace lab4.Models
{
    public class PurchaseOrderItem
    {
        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }
        [ValidateNever]
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; } = null!;

        public int QuantityOrdered { get; set; }

        // GIÁ THỎA THUẬN
        public decimal UnitPrice { get; set; }

        // theo dõi
        public int QuantityReceived { get; set; }
    }


}
