using System.Numerics;

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(RestockId), nameof(ItemId))]
    public class RestockItem
    {
        public RestockItem()
        {
        }

        public RestockItem(int itemId, int quantity, int restockId, decimal restockPrice, Restock restock, Item item)
        {
            ItemId = itemId;
            Quantity = quantity;
            RestockId = restockId;
            RestockPrice = restockPrice;
            Restock = restock;
            Item = item;
        }

        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int RestockId { get; set; }

        [Precision(5, 2)]
        public Decimal RestockPrice { get; set; }
        //References
        public Restock Restock { get; set; }
        public Item Item { get; set; }

    }
}
