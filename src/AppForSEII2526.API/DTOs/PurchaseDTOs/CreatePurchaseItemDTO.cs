
namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class CreatePurchaseItemDTO
    {
        public CreatePurchaseItemDTO() { }
        public CreatePurchaseItemDTO(int itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }
        public CreatePurchaseItemDTO(int itemId, int quantity, decimal price)
        {
            ItemId = itemId;
            Quantity = quantity;
            Price = price;
        }
        [Required(ErrorMessage = "ItemId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ItemId must be a positive number.")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public override bool Equals(object? obj)
        {
            return obj is CreatePurchaseItemDTO dTO &&
                   ItemId == dTO.ItemId &&
                   Quantity == dTO.Quantity &&
                   Price == dTO.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ItemId, Quantity, Price);
        }
    }
}