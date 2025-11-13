namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
           public class CreatePurchaseItemDTO
        {
            public CreatePurchaseItemDTO(int itemId, int quantity)
            {
                ItemId = itemId;
                Quantity = quantity;
            }
            [Required(ErrorMessage = "ItemId is required")]
            [Range(1, int.MaxValue, ErrorMessage = "ItemId must be a positive number.")]
            public int ItemId { get; set; }

            [Required(ErrorMessage = "Quantity is required")]
            [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
            public int Quantity { get; set; }
        }
    }
