using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace AppForSEII2526.API.Models
{

    [Index(nameof(Title), IsUnique = true)]
    [Index(nameof(Id), IsUnique = true)]
    

    public class Restock
    {
        private ApplicationUser? admin;

        public Restock()
        {}

        public Restock(string deliveryAddress, string? description, DateTime? expectedDate, DateTime restockDate, string title, decimal totalPrice, List<RestockItem> restockItems, ApplicationUser? admin)
        {
            DeliveryAddress = deliveryAddress;
            Description = description;
            ExpectedDate = expectedDate;
            RestockDate = restockDate;
            Title = title;
            TotalPrice = totalPrice;
            RestockItems = restockItems;
            this.admin = admin;
        }

        public Restock(ApplicationUser? admin, string? deliveryAddress, string description, DateTime? expectedDate, DateTime restockDate, string title, decimal totalPrice, string restockResponsibleId, IList<RestockItem> restockItems, ApplicationUser restockResponsible)
        {
            this.admin = admin;
            DeliveryAddress = deliveryAddress;
            Description = description;
            ExpectedDate = expectedDate;
            RestockDate = restockDate;
            Title = title;
            TotalPrice = totalPrice;
            RestockResponsibleId = restockResponsibleId;
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
        public string RestockResponsibleId { get; set; }  // FK obligatoria



    }
}
