using Lab4.Models;

namespace lab4.Models
{
    public class SupplierProduct
    {
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}