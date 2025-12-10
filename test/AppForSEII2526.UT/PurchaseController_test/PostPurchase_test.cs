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
            var user2 = new ApplicationUser()
            {
                Id = "2",
                UserName = "test2",
                Surname = "user2",
                Email = "test2@test.com",

            };
            _context.Users.AddRange(user2);
            var user3 = new ApplicationUser()
            {
                Id = "3",
                UserName = "test3",
                Surname = "user3",
                Email = "test3@test.com",

            };
            _context.Users.AddRange(user3);
            _context.SaveChanges();
            var paymentMethod = new List<PaymentMethod>()
            {
                new Bizum(){ Id= 1,User= user, TelephoneNumber= 664543223},
                new CreditCard(){ Id= 2,User= user2, CreditCardNumber = "123456789",ExpirationDate=new DateTime(2027,12,1)},
                new PayPal(){ Id= 3,User= user3, Email="test3@test.com" }

            };
            _context.PaymentMethod.AddRange(paymentMethod);
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
                   description: "My purchase for holidays",
                   purchaseItems: new List<CreatePurchaseItemDTO>()
                   {
                       new CreatePurchaseItemDTO(1, 2),
                   }
                );
            var purchaseInvalidItem = new ItemForCreateDTO(
                   customerUserName: "test2",
                   paymentMethodId: 2,
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "My purchase for holidays",
                   purchaseItems: new List<CreatePurchaseItemDTO>()
                   {
                       new CreatePurchaseItemDTO(999, 2),
                   }
                );
            var purchaseInsufficientStock = new ItemForCreateDTO(
                   customerUserName: "test3",
                   paymentMethodId: 3,
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "My purchase for holidays",
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
                   description: "My purchase for holidays",
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
                   description: "My purchase for holidays",
                   purchaseItems: new List<CreatePurchaseItemDTO>()
                   {
                       new CreatePurchaseItemDTO(1, 2),
                   }
                );
            var purchasewrongDescription = new ItemForCreateDTO(
                   customerUserName: "test",
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
            var purchaseinvalidquantity = new ItemForCreateDTO(
                   customerUserName: "test",
                   paymentMethodId: 1,
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "My purchase for holidays",
                   purchaseItems: new List<CreatePurchaseItemDTO>()
                   {
                       new CreatePurchaseItemDTO(1, -1),
                   }
                );

            var allTests = new List<object[]>
            {
                new object[] { purchaseInvalidPaymentMethod, "Error! The selected payment method is not registered for this user." },
                new object[] { purchaseInvalidItem,         "Error! Item with Id 999 not found." },
                new object[] { purchaseInsufficientStock,   "Error! Item Foam Roller does not have enough stock. Available: 100, Requested: 200" },
                new object[] { purchaseInvalidUser,         "Error! Username or email is not registred." },
                //new object[] { purchasewrongPM,         "Error! The selected payment method is not registered for this user." },
                new object[] { purchasewrongDescription,         "Error! You must start the Description with My purchase for." },
                new object[] { purchaseinvalidquantity,    "Error! Item Foam Roller has invalid quantity -1. Quantity must be greater than zero."
 }




           };
            return allTests;
        }
        [Theory]
        [MemberData(nameof(InvalidPostPurchaseInputs))]
        [Trait("PostPurchase", "Unit Testing")]
        public async Task PostPurchase_InvalidInput_ReturnsBadRequest(ItemForCreateDTO input, string errors)
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
                    customerUserName: "test",
                   paymentMethodId: 1,
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "My purchase for holidays",
                   purchaseItems: new List<CreatePurchaseItemDTO>()
                   {
                       new CreatePurchaseItemDTO(1, 2),
                   }
                );
            var result = await controller.CreateItemForPurchase(input);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsType<PurchaseDetailDTO>(created.Value);

            var expected = new PurchaseDetailDTO(
                   id: actual.Id,
                   paymentMethod: "Bizum",
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "My purchase for holidays",
                     purchaseItems: actual.PurchaseItems,
                     totalPrice: 20.0m
                );



            Assert.Equal(expected, actual);


        }
        [Fact]
        [Trait("PostPurchase", "Unit Testing")]
        public async Task PostPurchase_ReturnsCreatedDescriptionNull()
        {
            var mock = new Mock<ILogger<PurchaseController>>();
            ILogger<PurchaseController> logger = mock.Object;
            PurchaseController controller = new PurchaseController(_context, logger);

            var input = new ItemForCreateDTO(
                    customerUserName: "test3",
                   paymentMethodId: 3,
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "",
                   purchaseItems: new List<CreatePurchaseItemDTO>()
                   {
                       new CreatePurchaseItemDTO(1, 2),
                   }
                );
            var result = await controller.CreateItemForPurchase(input);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsType<PurchaseDetailDTO>(created.Value);

            var expected = new PurchaseDetailDTO(
                   id: actual.Id,
                   paymentMethod: "PayPal",
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "",
                     purchaseItems: actual.PurchaseItems,
                     totalPrice: 20.0m
                );

            Assert.Equal(expected, actual);

        }

           [Fact]
        [Trait("PostPurchase", "Unit Testing")]
        public async Task PostPurchase_ReturnsCreatedCreditCard()
        {
            var mock = new Mock<ILogger<PurchaseController>>();
            ILogger<PurchaseController> logger = mock.Object;
            PurchaseController controller = new PurchaseController(_context, logger);

            var input = new ItemForCreateDTO(
                    customerUserName: "test2",
                   paymentMethodId: 2,
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "My purchase for holidays",
                   purchaseItems: new List<CreatePurchaseItemDTO>()
                   {
                       new CreatePurchaseItemDTO(1, 2),
                   }
                );
            var result = await controller.CreateItemForPurchase(input);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsType<PurchaseDetailDTO>(created.Value);

            var expected = new PurchaseDetailDTO(
                   id: actual.Id,
                   paymentMethod: "Credit Card",
                   street: "C/Plaza Mayor",
                   city: "Albacete",
                   country: "Spain",
                   description: "My purchase for holidays",
                     purchaseItems: actual.PurchaseItems,
                     totalPrice: 20.0m
            );



            Assert.Equal(expected, actual);



        }
    }
}
