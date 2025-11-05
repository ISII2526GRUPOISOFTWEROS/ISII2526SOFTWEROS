namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class CreatePurchaseItemDTO
    {
        public CreatePurchaseItemDTO(int itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }
        [Required(ErrorMessage = "At least one item must be included in the purchase")]
        [Range(1,int.MaxValue, ErrorMessage = "Purchase Item List must containt at least one item")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "At least one item must be included in the purchase")]
        [Range(1, int.MaxValue,ErrorMessage = "Purchase Item List must containt at least one item")]
        public int Quantity { get; set; }
    }
}
