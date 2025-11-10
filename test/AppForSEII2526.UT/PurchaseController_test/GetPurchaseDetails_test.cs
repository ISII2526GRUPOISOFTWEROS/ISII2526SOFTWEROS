using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AppForSEII2526.UT.PurchaseController_test
{
    public class GetPurchaseDetails_test : AppForSEII25264SqliteUT
    {
        private readonly int existingPurchaseId;

        public GetPurchaseDetails_test()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                UserName = "test",
                Surname ="user",
                Email = "test@test.com",
            };
            _context.Users.AddRange(user);
            _context.SaveChanges();
            var paymentMethods = new List<Bizum>()
            {
                new Bizum(){ Id= 1,User= user, TelephoneNumber= 664543223},

            };
            _context.Bizums.AddRange(paymentMethods);
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

            var purchase = new Purchase()
            {
                PaymentMethod = paymentMethods[0],
                Street = "C/Plaza Mayor",
                City = "Albacete",
                Country = "Spain",
                Description = "First Purchase",
                Total_prices = 80.0m,
                PurchaseItems = new List<PurchaseItem>()
                {
                    new PurchaseItem(){ Item = items[0], Price = 50.0m, Amount_bought = 1 },
                    new PurchaseItem(){ Item = items[1], Price = 30.0m, Amount_bought = 1 },
                }
            };
            _context.Purchases.Add(purchase);
            _context.SaveChanges();

           

            existingPurchaseId = purchase.Id;
        }

        

       
        [Fact]
        [Trait("GetPurchaseDetails", "Unit Testing")]
        public async Task GetPurchaseDetails_NotFound_Test()
        {
            var mock = new Mock<ILogger<PurchaseController>>();
            ILogger<PurchaseController> logger = mock.Object;

            PurchaseController controller = new PurchaseController(_context, logger);

            var result = await controller.GetPurchaseDetails(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("GetPurchaseDetails", "Unit Testing")]
        public async Task GetPurchaseDetails_Successful_Test()
        {
            var expectedDetails = new PurchaseDetailDTO(
                existingPurchaseId,
                "Bizum",
                "C/Plaza Mayor",
                "Albacete",
                "Spain",
                "First Purchase",
                new List<PurchasedItemDTO>()
                {
                    new PurchasedItemDTO( "Foam Roller", "Nike", 50.0m, 1),
                    new PurchasedItemDTO( "Bands", "Domyos", 30.0m, 1),
                },
                80.0m
            );

            var mock = new Mock<ILogger<PurchaseController>>();
            ILogger<PurchaseController> logger = mock.Object;

            PurchaseController controller = new PurchaseController(_context, logger);

            var result = await controller.GetPurchaseDetails(existingPurchaseId);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var purchaseDetail = Assert.IsType<List<PurchaseDetailDTO>>(okResult.Value);
        
            var detail = Assert.Single(purchaseDetail);

            Assert.Equal(expectedDetails.Id, detail.Id);
            Assert.Equal(expectedDetails.PaymentMethod, detail.PaymentMethod);
            Assert.Equal(expectedDetails.DeliveryAddress, detail.DeliveryAddress);
            Assert.Equal(expectedDetails.Description, detail.Description);
            Assert.Equal(expectedDetails.TotalPrice, detail.TotalPrice);
            Assert.Equal(expectedDetails.PurchaseItems.Count, detail.PurchaseItems.Count);

            var expectedItem = expectedDetails.PurchaseItems.OrderBy(i => i.Name).ToList();
            var actualItem = detail.PurchaseItems.OrderBy(i => i.Name).ToList();

            Assert.Equal(expectedItem.Count, actualItem.Count);

            for (int i = 0; i < expectedDetails.PurchaseItems.Count; i++)
            {
                Assert.Equal(expectedDetails.PurchaseItems[i].Name, detail.PurchaseItems[i].Name);
                Assert.Equal(expectedDetails.PurchaseItems[i].Brand, detail.PurchaseItems[i].Brand);
                Assert.Equal(expectedDetails.PurchaseItems[i].Price, detail.PurchaseItems[i].Price);
                Assert.Equal(expectedDetails.PurchaseItems[i].Quantity, detail.PurchaseItems[i].Quantity);
            }
        }

    }
}
