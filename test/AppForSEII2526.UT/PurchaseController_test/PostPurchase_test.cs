using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AppForSEII2526.UT.PurchaseController_test
{
    public class PostPurchase_test : AppForSEII25264SqliteUT
    {
        private readonly int existingPurchaseId;

        public PostPurchase_test()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                UserName = "test",
                Surname = "user",
                Email = "test@test.com",
            };
            _context.Users.AddRange(user);
            _context.SaveChanges();
            var paymentMethod = new List<Bizum>()
            {
                new Bizum(){ Id= 1,User= user, TelephoneNumber= 664543223},

            };
            _context.Bizums.AddRange(paymentMethod);
            _context.SaveChanges();

            var brands = new List<Brand>()
            {
                new Brand(){ Name="Nike"},
                new Brand(){ Name="Domyos"},
            };
            _context.Brands.AddRange(brands);
            _context.SaveChanges();

            var itemTypes = new List<ItemType>()
            {
                new ItemType(){ Name="Strength Equipment"},
                new ItemType(){ Name="Cardio Equipment"},
            };

            _context.ItemTypes.AddRange(itemTypes);
            _context.SaveChanges();

            var items = new List<Item>()
            {
                new Item(){Name="Foam Roller", Brand=brands[0], Description="Description1", PurchasePrice=10.0m, QuantityAvailableForPurchase=100,ItemType = itemTypes[1]},
                new Item(){ Name="Bands", Brand=brands[1], Description="Description2", PurchasePrice=20.0m, QuantityAvailableForPurchase=200,ItemType = itemTypes[0]},
                new Item(){ Name="Kettlebell", Brand=brands[0], Description="Description3", PurchasePrice=30.0m, QuantityAvailableForPurchase=300,ItemType = itemTypes[0]},


            };
            _context.Items.AddRange(items);
            _context.SaveChanges();

        }

        [Fact]
        [Trait("PostPurchase", "Unit Testing")]
        public async Task PostPurchase_ReturnsCreated()
        {
            var introduce = new ItemForCreateDTO(
                   paymentMethodId: 1,
                   street: "C/Plaza Mayor",
                   city:"Albacete",
                   country:"Spain",
                   description:"First purchase",
                   items: new List<ItemForPurchaseDTO>
                   {
                            new ItemForPurchaseDTO(),
                            new ItemForPurchaseDTO(),
        
                   }
                );
        }






    }
}
