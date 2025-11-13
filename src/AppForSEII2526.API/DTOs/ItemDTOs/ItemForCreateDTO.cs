
using AppForSEII2526.API.DTOs.PurchaseDTOs;
namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForCreateDTO
    {
        public ItemForCreateDTO()
        {
        }

        public ItemForCreateDTO(int paymentMethodId, string street, string city, string country, string description, IList<CreatePurchaseItemDTO> purchaseItems)
        {
            PaymentMethodId = paymentMethodId;
            Street = street;
            City = city;
            Country = country;
            Description = description;
            PurchaseItems = purchaseItems;
        }


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
        public IList<CreatePurchaseItemDTO> PurchaseItems { get; set; }




        public override bool Equals(object? obj)
        {
            return obj is ItemForCreateDTO dTO &&
                   PaymentMethodId == dTO.PaymentMethodId &&
                   Street == dTO.Street &&
                   City == dTO.City &&
                   Country == dTO.Country &&
                   Description == dTO.Description &&
                   EqualityComparer<IList<CreatePurchaseItemDTO>>.Default.Equals(PurchaseItems, dTO.PurchaseItems);
        }
    }

}
