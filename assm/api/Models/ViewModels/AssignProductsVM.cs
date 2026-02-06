namespace lab4.Models.ViewModels
{
    public class AssignProductsVM
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = "";

        public List<ProductItem> Products { get; set; } = new();
    }

    public class ProductItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public bool Selected { get; set; }
    }
}