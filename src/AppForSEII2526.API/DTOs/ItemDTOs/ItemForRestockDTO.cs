namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForRestockDTO
    {
        public ItemForRestockDTO(int id, string brand, string name)
        {
            Id = id;
            Brand = brand;
            Name = name;
            
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string Brand { get; set; }
        public int QuantityAvailableForPurchase { get; set; }

        [Precision(10, 2)]
        public decimal RestockPrice { get; set; }
    }
}
