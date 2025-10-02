using System.Numerics;

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ItemId),
 nameof(RestockId))]
    public class RestockItem
    {
        
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
