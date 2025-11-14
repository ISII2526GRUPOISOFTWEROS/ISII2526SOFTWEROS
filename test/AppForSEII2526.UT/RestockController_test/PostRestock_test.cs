using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RestockDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForSEII2526.UT.RestockController_test
{
    public class PostRestock_test : AppForSEII25264SqliteUT
    {
        public PostRestock_test()
        {
            // Usuario responsable del restock
            var admin = new ApplicationUser()
            {
                Id = "1",
                UserName = "admin",
                Surname = "responsable",
                Email = "admin@test.com",
            };
            _context.Users.Add(admin);

            // Items en inventario
            var brands = new List<Brand>()
            {
                new Brand() { Name = "Nike" },
                new Brand() { Name = "Domyos" },

            }
            ;

            var itemTypes = new List<ItemType>()
            {
                new ItemType(){ Name="Strength Equipment"},
                new ItemType(){ Name="Cardio Equipment"},
            };


            var items = new List<Item>()
            {
                new Item(){ Name="Foam Roller", QuantityForRestock=30, Brand= brands[0], QuantityAvailableForPurchase=25 ,ItemType = itemTypes[0]},
                new Item(){ Name="Bands", QuantityForRestock=20, Brand=brands[1], QuantityAvailableForPurchase=15 ,ItemType = itemTypes[1]},
                new Item(){ Name="Kettlebell", QuantityForRestock=30, Brand=brands[0], QuantityAvailableForPurchase=25 ,ItemType = itemTypes[0]},

            };


            _context.Items.AddRange(items);
            _context.Brands.AddRange(brands);
            _context.ItemTypes.AddRange(itemTypes);

            _context.SaveChanges();
        }

        // --------------------------
        //   TESTS CON INPUT INVÁLIDO
        // --------------------------
        public static IEnumerable<object[]> InvalidRestockInputs()
        {
            var missingTitle = new ItemForCreateRestockDTO(
                id: 0,
                title: "",
                deliveryAddress: "Warehouse",
                description: "Description",
                expectedDate: DateTime.Now,
                restockDate: DateTime.Now,
                totalPrice: 0,
                restockItems: new List<RestockItemForCreateDTO>()
                {
                    new RestockItemForCreateDTO("Foam Roller", 1, 10, 2)
                },
                restockResponsible: "admin"
            );

            var missingAddress = new ItemForCreateRestockDTO(
                id: 0,
                title: "Restock 1",
                deliveryAddress: "",
                description: "Description",
                expectedDate: DateTime.Now,
                restockDate: DateTime.Now,
                totalPrice: 0,
                restockItems: new List<RestockItemForCreateDTO>()
                {
                    new RestockItemForCreateDTO("Foam Roller", 1, 10, 5)
                },
                restockResponsible: "admin"
            );

            var missingItems = new ItemForCreateRestockDTO(
                id: 0,
                title: "Restock 1",
                deliveryAddress: "Warehouse",
                description: "Description",
                expectedDate: DateTime.Now,
                restockDate: DateTime.Now,
                totalPrice: 0,
                restockItems: new List<RestockItemForCreateDTO>(),
                restockResponsible: "admin"
            );

            var invalidUser = new ItemForCreateRestockDTO(
                id: 0,
                title: "Restock 1",
                deliveryAddress: "Warehouse",
                description: "Description",
                expectedDate: DateTime.Now,
                restockDate: DateTime.Now,
                totalPrice: 0,
                restockItems: new List<RestockItemForCreateDTO>()
                {
                    new RestockItemForCreateDTO("Foam Roller", 1, 5, 2)
                },
                restockResponsible: "unknownUser"
            );

            return new List<object[]>
            {
                new object[] { missingTitle, "Error! Title is required." },
                new object[] { missingAddress, "Error! Delivery Address is required." },
                new object[] { missingItems, "Error! At least one restock item is required." },
                new object[] { invalidUser, "The user responsible for the restock does not exist." }
            };
        }

        [Theory]
        [MemberData(nameof(InvalidRestockInputs))]
        [Trait("PostRestock", "Unit Testing")]
        public async Task PostRestock_InvalidInput_ReturnsBadRequest(ItemForCreateRestockDTO restockDTO, string expectedError)
        {
            //Arrange
            var mock = new Mock<ILogger<RestockController>>();
            ILogger<RestockController> logger = mock.Object;

            var controller = new RestockController(_context, logger);


            //Act
            var result = await controller.CreateRestock(restockDTO);

            //Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequest.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.Equal(expectedError, errorActual);
        }

        // --------------------------
        //   TEST CON INPUT VÁLIDO
        // --------------------------
        [Fact]
        [Trait("PostRestock", "Unit Testing")]
        public async Task PostRestock_ValidInput_ReturnsCreated()
        {
            //Arrange
            var mock = new Mock<ILogger<RestockController>>();
            ILogger<RestockController> logger = mock.Object;

            var controller = new RestockController(_context, logger);


            var input = new ItemForCreateRestockDTO(
                id: 0,
                title: "Restock Test",
                deliveryAddress: "Warehouse A",
                description: "Restocking items",
                expectedDate: DateTime.UtcNow,
                restockDate: DateTime.UtcNow,
                totalPrice: 0,
                restockItems: new List<RestockItemForCreateDTO>()
                {
                    new RestockItemForCreateDTO("Foam Roller", 1, 10, 2)
                },
                restockResponsible: "admin"
            );

            //Act
            var result = await controller.CreateRestock(input);


            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualCreatedRestock = Assert.IsType<ItemForCreateRestockDTO>(createdResult.Value);

            Assert.Equal(input, actualCreatedRestock);
        }
    }
}
