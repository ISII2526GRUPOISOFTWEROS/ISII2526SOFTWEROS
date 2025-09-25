namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        //Primary key 
        public int Id { get; set; } 

        //Strings
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        //Decimal
        public decimal Total_prices { get; set; }
    }
}
