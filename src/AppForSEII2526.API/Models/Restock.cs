using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace AppForSEII2526.API.Models
{

    [Index(nameof(Title), IsUnique = true)]
    [Index(nameof(Id), IsUnique = true)]
    

    public class Restock
    {
        public Restock()
        {}

        public Restock(string? deliveryAddress, string description, DateTime? expectedDate, int id, DateTime restockDate, string title, decimal totalPrice, IList<RestockItem> restockItems, ApplicationUser restockResponsible)
        {
            DeliveryAddress = deliveryAddress;
            Description = description;
            ExpectedDate = expectedDate;
            Id = id;
            RestockDate = restockDate;
            Title = title;
            TotalPrice = totalPrice;
            RestockItems = restockItems;
            RestockResponsible = restockResponsible;
        }

        public string? DeliveryAddress { get; set; }
        public string Description { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public int Id { get; set; }
        public DateTime RestockDate { get; set; }
        public string Title { get; set; }
        [Precision(5, 2)]
        public Decimal TotalPrice { get; set; }

        //References
        public IList<RestockItem> RestockItems { get; set; }
        public ApplicationUser RestockResponsible { get; set; }
        


    }
}
