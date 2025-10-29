namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForRestockDTO
    {
        public ItemForRestockDTO(int id, string brand, string name, int quantityavailablerestock, decimal restockprice)
        {
            Id = id;
            Brand = brand;
            Name = name;
            QuantityAvailableForRestock = quantityavailablerestock;
            RestockPrice = restockprice;

        }
        

        public int Id { get; set; }
        public string? Name { get; set; }
        public string Brand { get; set; }
        public int QuantityAvailableForRestock { get; set; }

        [Precision(10, 2)]
        public decimal RestockPrice { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemForRestockDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   QuantityAvailableForRestock == dTO.QuantityAvailableForRestock &&
                   RestockPrice == dTO.RestockPrice;
        }
    }
}
