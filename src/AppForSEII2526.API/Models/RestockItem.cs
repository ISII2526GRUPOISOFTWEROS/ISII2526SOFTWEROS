using System.Numerics;

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(RestockId), nameof(ItemId))]
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
        public DbSet<Bizum> Bizums { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<PayPal> PayPals { get; set; }

    }
}
