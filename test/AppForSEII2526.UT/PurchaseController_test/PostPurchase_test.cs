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
        public static IEnumerable<object[]> InvalidPostPurchaseInputs()
        {
           
            var purchaseInvalidPaymentMethod = new ItemForCreateDTO(
                   customerUserName: "test",
                   paymentMethodId: 999,
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "First purchase",
                   purchaseItems: new List<CreatePurchaseItemDTO>()
                   {
                       new CreatePurchaseItemDTO(1, 2),
                   }
                );
            var purchaseInvalidItem = new ItemForCreateDTO(
                   customerUserName: "test",
                   paymentMethodId: 1,
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "First purchase",
                   purchaseItems: new List<CreatePurchaseItemDTO>()
                   {
                       new CreatePurchaseItemDTO(999, 2),
                   }
                );
            var purchaseInsufficientStock = new ItemForCreateDTO(
                   customerUserName: "test",
                   paymentMethodId: 1,
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "First purchase",
                   purchaseItems: new List<CreatePurchaseItemDTO>()
                   {
                       new CreatePurchaseItemDTO(1, 200),
                   }
                );
            var purchaseInvalidUser = new ItemForCreateDTO(
                   customerUserName: "nonexistentuser",
                   paymentMethodId: 1,
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "First purchase",
                   purchaseItems: new List<CreatePurchaseItemDTO>()
                   {
                       new CreatePurchaseItemDTO(1, 2),
                   }
                );
            var purchasewrongPM = new ItemForCreateDTO(
                   customerUserName: "test",
                   paymentMethodId: 2,
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "First purchase",
                   purchaseItems: new List<CreatePurchaseItemDTO>()
                   {
                       new CreatePurchaseItemDTO(1, 2),
                   }
                );

            var allTests = new List<object[]>
            {
                new object[] { purchaseInvalidPaymentMethod, "Error!" },
                new object[] { purchaseInvalidItem,         "Error!" },
                new object[] { purchaseInsufficientStock,   "Error!" },
                new object[] { purchaseInvalidUser,         "Error!" },
                new object[] { purchasewrongPM,         "Error!" },

           };
            return allTests;
        }
        [Theory]
        [MemberData(nameof(InvalidPostPurchaseInputs))]
        [Trait("PostPurchase", "Unit Testing")]
        public async Task PostPurchase_InvalidInput_ReturnsBadRequest(ItemForCreateDTO input,string errors)
        {
            var mock = new Mock<ILogger<PurchaseController>>();
            ILogger<PurchaseController> logger = mock.Object;

            PurchaseController controller = new PurchaseController(_context, logger);
            
            var result = await controller.CreateItemForPurchase(input);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var innerResult = Assert.IsType<ObjectResult>(badRequest.Value);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(innerResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errors, errorActual);
        }

        [Fact]
        [Trait("PostPurchase", "Unit Testing")]
        public async Task PostPurchase_ReturnsCreated()
        {
            var mock = new Mock<ILogger<PurchaseController>>();
            ILogger<PurchaseController> logger = mock.Object;
            PurchaseController controller = new PurchaseController(_context, logger);

            var input = new ItemForCreateDTO(
                    customerUserName:"test",
                   paymentMethodId: 1,
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "First purchase",
                   purchaseItems: new List<CreatePurchaseItemDTO>()
                   {
                       new CreatePurchaseItemDTO(1, 2),
                   }
                );

            var expected = new PurchaseDetailDTO(
                   id: 1,
                   paymentMethod: "Bizum",
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "First purchase",
                     purchaseItems: new List<PurchasedItemDTO>()
                     {
                          new PurchasedItemDTO( "Foam Roller", "Nike",10.0m, 2),
                     }
                     , totalPrice: 20.0m
                );


            var result = await controller.CreateItemForPurchase(input);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsType<PurchaseDetailDTO>(created.Value);

            Assert.True(actual.Id > 0);

            Assert.Equal("Bizum", actual.PaymentMethod);
            Assert.Equal("C/Plaza Mayor", actual.Street);
            Assert.Equal("Albacete", actual.City);
            Assert.Equal("Spain", actual.Country);
            Assert.Equal("First purchase", actual.Description);
            Assert.Equal(20.0m, actual.TotalPrice);

            var items = actual.PurchaseItems;
            Assert.Single(items);

            var item = items[0];
            Assert.Equal("Foam Roller", item.Name);
            Assert.Equal("Nike", item.Brand);
            Assert.Equal(10.0m, item.Price);
            Assert.Equal(2, item.Quantity);

        }
    }
}
