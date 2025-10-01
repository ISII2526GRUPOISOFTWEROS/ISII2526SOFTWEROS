using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace AppForSEII2526.API.Models
{

    [Index(nameof(Title), IsUnique = true)]
    [Index(nameof(Id), IsUnique = true)]
    public class Restock
    {
        public string DeliveryAddress { get; set; }
        public string Description { get; set; }
        public DateTime ExpectedDate { get; set; }
        public int Id { get; set; }
        public DateTime RestockDate { get; set; }
        public string Title { get; set; }
        [Precision(5, 2)]
        public Decimal TotalPrice { get; set; }
        public IList<RestockItem> RestockItems { get; set; }


    }
}
