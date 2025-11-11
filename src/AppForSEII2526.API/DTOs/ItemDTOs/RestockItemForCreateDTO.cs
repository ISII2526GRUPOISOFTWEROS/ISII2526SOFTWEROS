namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class RestockItemForCreateDTO
    {
        public RestockItemForCreateDTO(string itemName, int itemId, int quantity, decimal restockPrice)
        {
            ItemName = itemName;
            ItemId = itemId;
            Quantity = quantity; // El tipo int no puede ser null, así que no se necesita el operador ?? ni la excepción.
            RestockPrice = restockPrice;
        }

        public string ItemName { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }
        public decimal RestockPrice { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RestockItemForCreateDTO dTO &&
                   ItemName == dTO.ItemName &&
                   ItemId == dTO.ItemId &&
                   Quantity == dTO.Quantity &&
                   RestockPrice == dTO.RestockPrice;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ItemName, ItemId, Quantity, RestockPrice);
        }
    }
}
