using Lab4.Models;

namespace lab4.Models
{
    public class GoodsReceiptItem
    {
        public int Id { get; set; }

        public int GoodsReceiptId { get; set; }
        public GoodsReceipt GoodsReceipt { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int QuantityReceived { get; set; }
        public decimal UnitCost { get; set; }
    }


}
