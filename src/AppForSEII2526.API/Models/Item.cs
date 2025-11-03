namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    [PrimaryKey(nameof(Id))]
    public class Item
    {
        public Item()
        {
        }

        public Item(int id, string? description, string? name, int quantityAvailableForPurchase, int quantityForRestock, decimal restockPrice, decimal purchasePrice, ItemType itemType, Brand brand)
        {
            Id = id;
            Description = description;
            Name = name;
            QuantityAvailableForPurchase = quantityAvailableForPurchase;
            QuantityForRestock = quantityForRestock;
            RestockPrice = restockPrice;
            PurchasePrice = purchasePrice;
            ItemType = itemType;
            Brand = brand;
        }


        public Item(int id, string? description, string? name, IList<PurchaseItem> purchaseItems, int quantityAvailableForPurchase, int quantityForRestock, decimal restockPrice, decimal purchasePrice, IList<RestockItem> restockItems, ItemType itemType, Brand brand)
        {
            Id = id;
            Description = description;
            Name = name;
            PurchaseItems = purchaseItems;
            QuantityAvailableForPurchase = quantityAvailableForPurchase;
            QuantityForRestock = quantityForRestock;
            RestockPrice = restockPrice;
            PurchasePrice = purchasePrice;
            RestockItems = restockItems;
            ItemType = itemType;
            Brand = brand;
        }


        public Item(int id, string? description, string? name, int quantityAvailableForPurchase, int quantityForRestock, decimal restockPrice, decimal purchasePrice, ItemType itemType, Brand brand)
        {
            Id = id;
            Description = description;
            Name = name;
            QuantityAvailableForPurchase = quantityAvailableForPurchase;
            QuantityForRestock = quantityForRestock;
            RestockPrice = restockPrice;
            PurchasePrice = purchasePrice;
            ItemType = itemType;
            Brand = brand;
        }

        public int Id { get; set; }
        public string? Description  { get; set; }
        public string? Name { get; set; }
        public IList<PurchaseItem> PurchaseItems { get; set; }
        public int QuantityAvailableForPurchase { get; set; }
        public int QuantityForRestock { get; set; }
        [Precision(10, 2)] 
        public decimal RestockPrice { get; set; }
        [Precision(10, 2)] 
        public decimal PurchasePrice { get; set; }

        //Reference
        public IList<RestockItem> RestockItems { get; set; }

        public ItemType ItemType { get; set; }
        public Brand Brand { get; set; }
    }
}
