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


            _context.Brands.AddRange(brands);
            _context.ItemTypes.AddRange(itemTypes);
            _context.Items.AddRange(items);
            

            _context.SaveChanges();
        }

        // --------------------------
        //   TESTS CON INPUT INVÁLIDO
        // --------------------------
        public static IEnumerable<object[]> InvalidRestockInputs()
        {
            var missingTitle = new RestockForCreateDTO(
                
                title: "",
                deliveryAddress: "Warehouse",
                description: "Restock for",
                expectedDate: DateTime.Now,
                restockDate: DateTime.Now,
                totalPrice: 0,
                restockItems: new List<RestockItemDTO>()
                {
                    new RestockItemDTO("Foam Roller", 1, 10, 2)
                },
                restockResponsible: "admin"
            );

            var missingAddress = new RestockForCreateDTO(
                
                title: "Restock 1",
                deliveryAddress: "",
                description: "Restock for",
                expectedDate: DateTime.Now,
                restockDate: DateTime.Now,
                totalPrice: 0,
                restockItems: new List<RestockItemDTO>()
                {
                    new RestockItemDTO("Foam Roller", 1, 10, 5)
                },
                restockResponsible: "admin"
            );

            var missingItems = new RestockForCreateDTO(
                
                title: "Restock 1",
                deliveryAddress: "Warehouse",
                description: "Restock for",
                expectedDate: DateTime.Now,
                restockDate: DateTime.Now,
                totalPrice: 0,
                restockItems: new List<RestockItemDTO>(),
                restockResponsible: "admin"
            );

            var invalidUser = new RestockForCreateDTO(
                
                title: "Restock 1",
                deliveryAddress: "Warehouse",
                description: "Restock for",
                expectedDate: DateTime.Now,
                restockDate: DateTime.Now,
                totalPrice: 0,
                restockItems: new List<RestockItemDTO>()
                {
                    new RestockItemDTO("Foam Roller", 1, 5, 2)
                },
                restockResponsible: "unknownUser"
            );
            var incorrectDescription = new RestockForCreateDTO(
                
                title: "Restock 1",
                deliveryAddress: "Warehouse",
                description: "Description",
                expectedDate: DateTime.Now,
                restockDate: DateTime.Now,
                totalPrice: 0,
                restockItems: new List<RestockItemDTO>()
                {
                    new RestockItemDTO("Foam Roller", 1, 5, 2)
                },
                restockResponsible: "admin"
            );

            return new List<object[]>
            {
                new object[] { missingTitle, "Error! Title is required." },
                new object[] { missingAddress, "Error! Delivery Address is required." },
                new object[] { missingItems, "Error! At least one restock item is required." },
                new object[] { invalidUser, "The user responsible for the restock does not exist." },
                new object[] { incorrectDescription, "Error! You must start the Description with 'Restock for'" }
            };
        }

        [Theory]
        [MemberData(nameof(InvalidRestockInputs))]
        [Trait("PostRestock", "Unit Testing")]
        public async Task PostRestock_InvalidInput_ReturnsBadRequest(RestockForCreateDTO restockDTO, string expectedError)
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


            var input = new RestockForCreateDTO(
                
                title: "Restock Test",
                deliveryAddress: "Warehouse A",
                description: "Restock for",
                expectedDate: DateTime.UtcNow,
                restockDate: DateTime.UtcNow,
                totalPrice: 0,
                restockItems: new List<RestockItemDTO>()
                {
                    new RestockItemDTO("Foam Roller", 1, 10, 2)
                },
                restockResponsible: "admin"
            );

            //Act
            var result = await controller.CreateRestock(input);


            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualCreatedRestock = Assert.IsType<RestockForCreateDTO>(createdResult.Value);

            Assert.Equal(input, actualCreatedRestock);
        }
    }
}
