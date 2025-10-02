namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(Id))]
    public class Item
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int QuantityAvailableForPurchase { get; set; }
        public int QuantityForRestock { get; set; }
        [Precision(10, 2)] 
        public decimal RestockPrice { get; set; }
        [Precision(10, 2)] 
        public decimal PurchasePrice { get; set; }

        //Reference
        public IList<PurchaseItem> PurchaseItems { get; set; }
        public IList<RestockItem> RestockItems { get; set; }

        public ItemType ItemType { get; set; }
        public Brand Brand { get; set; }
    }
}
