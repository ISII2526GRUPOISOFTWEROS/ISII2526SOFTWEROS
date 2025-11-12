
namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForCreateDTO
    {
        public ItemForCreateDTO()
        {
        }

        public ItemForCreateDTO(int paymentMethodId, string street, string city, string country, string description, IList<ItemForPurchaseDTO> purchaseItems, decimal totalPrice)
        {
            PaymentMethodId = paymentMethodId;
            Street = street;
            City = city;
            Country = country;
            Description = description;
            PurchaseItems = purchaseItems;
            TotalPrice = totalPrice;
        }

        public ItemForCreateDTO(string customerUserName, int paymentMethodId, string street, string city, string country, string description, IList<ItemForPurchaseDTO> purchaseItems, decimal totalPrice)
        {
            CustomerUserName = customerUserName;
            PaymentMethodId = paymentMethodId;
            Street = street;
            City = city;
            Country = country;
            Description = description;
            PurchaseItems = purchaseItems;
            TotalPrice = totalPrice;
        }
        [Required]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Name must have at least 10 characters")]
        public string CustomerUserName { get; set; }

        [Required(ErrorMessage = "PaymentMethod is requiered")]
        [Range(1, int.MaxValue, ErrorMessage = "A valid PaymentMethod must be greater than 0")]
        public int PaymentMethodId { get; set; }


        [Required(ErrorMessage = "Street is requiered")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Street must have between 3 and 100 characters")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is requiered")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "City must have between 3 and 100 characters")] 
        public string City { get; set; }


        [Required(ErrorMessage = "Country is requiered")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Country must have between 3 and 100 characters")]
        public string Country { get; set; }


        [StringLength(100,  ErrorMessage = "Description cannot exceed 100 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "At least one item must be included in the purchase")]
        [MinLength(1, ErrorMessage = "Purchase Item List must containt at least one item")]
        public IList<ItemForPurchaseDTO> PurchaseItems { get; set; }


        [Required(ErrorMessage = "Total Price is requiered")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Total price must be greater than 0")]
        public decimal TotalPrice { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemForCreateDTO dTO &&
                   CustomerUserName == dTO.CustomerUserName &&
                   PaymentMethodId == dTO.PaymentMethodId &&
                   Street == dTO.Street &&
                   City == dTO.City &&
                   Country == dTO.Country &&
                   Description == dTO.Description &&
                   EqualityComparer<IList<ItemForPurchaseDTO>>.Default.Equals(PurchaseItems, dTO.PurchaseItems) &&
                   TotalPrice == dTO.TotalPrice;
        }
    }

}
