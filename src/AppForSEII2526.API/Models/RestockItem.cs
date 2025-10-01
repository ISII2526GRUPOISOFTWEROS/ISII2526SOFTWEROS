namespace AppForSEII2526.API.Models
{
    public class RestockItem
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int RestockId { get; set; }

        [Precision(5, 2)]
        public Decimal RestockPrice { get; set; }
        public Restock restock { get; set; }
    }
}
