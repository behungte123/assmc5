namespace lab4.Models
{
    public class GoodsReceiptViewModel
    {
        public int PurchaseOrderId { get; set; }

        public List<GoodsReceiptItemVM> Items { get; set; } = new();
    }

    public class GoodsReceiptItemVM
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
    }


}
