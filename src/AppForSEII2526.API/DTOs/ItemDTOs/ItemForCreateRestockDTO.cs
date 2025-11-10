

namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForCreateRestockDTO
    {
        public ItemForCreateRestockDTO()
        {
        }

        public ItemForCreateRestockDTO(int id, string title, string deliveryAddress, string? description, DateTime? expectedDate, DateTime restockDate, decimal totalPrice, RestockItemForCreateDTO restockItems, string restockResponsible)
        {
            Id = id;
            Title = title;
            DeliveryAddress = deliveryAddress;
            Description = description;
            ExpectedDate = expectedDate;
            RestockDate = restockDate;
            TotalPrice = totalPrice;
            RestockItems = (IList<RestockItemForCreateDTO>)restockItems;
            RestockResponsible = restockResponsible;
        }



        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string DeliveryAddress { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime RestockDate { get; set; }
        [Precision(5, 2)]
        public Decimal TotalPrice { get; set; }

        //References
        public IList<RestockItemForCreateDTO> RestockItems { get; set; } 
        public string RestockResponsible { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemForCreateRestockDTO dTO &&
                   Id == dTO.Id &&
                   Title == dTO.Title &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   Description == dTO.Description &&
                   ExpectedDate == dTO.ExpectedDate &&
                   RestockDate == dTO.RestockDate &&
                   TotalPrice == dTO.TotalPrice &&
                   EqualityComparer<IList<RestockItemForCreateDTO>>.Default.Equals(RestockItems, dTO.RestockItems) &&
                   RestockResponsible == dTO.RestockResponsible;
        }
    }



}
