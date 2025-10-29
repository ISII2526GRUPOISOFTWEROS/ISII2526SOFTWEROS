using Microsoft.Identity.Client;

namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        public Purchase()
        {
        }
        public Purchase(int id, string city, string country, DateTime date, string? description, string street, decimal total_prices)
        {
            Id = id;
            City = city;
            Country = country;
            Date = date;
            Description = description;
            Street = street;
            Total_prices = total_prices;
        }
        //Primary key 
        public int Id { get; set; } 

        //Strings
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string Street { get; set; }
        //Decimal
        [Precision(10, 2)] 
        public decimal Total_prices { get; set; }

        //Reference
        public IList<PurchaseItem> PurchaseItems { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
