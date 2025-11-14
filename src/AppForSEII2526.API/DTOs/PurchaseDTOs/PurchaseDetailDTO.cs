namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseDetailDTO 
    {

        public PurchaseDetailDTO(int id, string paymentMethod, string street, string city, string country, string description, IList<PurchasedItemDTO> purchaseItems, decimal totalPrice) 
        {
            Id = id;
            PaymentMethod = paymentMethod;
            Street = street;
            City = city;
            Country = country;
            Description = description;
            PurchaseItems = purchaseItems;
            TotalPrice = totalPrice;
        }
      


        public int Id { get; set; }

        public string PaymentMethod { get; set; }

        public string DeliveryAddress => $"{Street}, {City}, {Country}";

        [JsonIgnore]
        public string Street { get; set; }

        [JsonIgnore]
        public string City { get; set; }

        [JsonIgnore]
        public string Country { get; set; }

        public string? Description { get; set; }

        public IList<PurchasedItemDTO> PurchaseItems { get; set; }

        public decimal TotalPrice { get; set; }

   

     
    }
}
