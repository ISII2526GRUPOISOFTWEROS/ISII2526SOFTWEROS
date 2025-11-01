namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(PurchaseId), nameof(ItemId))]
    public class PurchaseItem
    {
        public PurchaseItem()
        {
        }
        public PurchaseItem(int itemId, int amount_bought, int purchaseId, decimal price)
        {
            ItemId = itemId;
            Amount_bought = amount_bought;
            PurchaseId = purchaseId;
            Price = price;
        }

        public int ItemId { get; set; }
        public int Amount_bought { get; set; }
        public int PurchaseId { get; set; }
        [Precision(10, 2)] 
        public decimal Price { get; set; }

        //Reference 
        public Purchase Purchase { get; set; }
        public Item Item { get; set; }

    }
}
