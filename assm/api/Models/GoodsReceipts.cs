namespace lab4.Models
{
    public class GoodsReceipt
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;

        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public DateTime ReceiptDate { get; set; }

        public ICollection<GoodsReceiptItem> Items { get; set; } = new List<GoodsReceiptItem>();
    }


}
