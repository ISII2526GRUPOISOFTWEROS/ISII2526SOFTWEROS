using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RestockDTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.RestockController_test
{
    public class GetRestockDetails_test : AppForSEII25264SqliteUT
    {

        public GetRestockDetails_test()
        {
            // Usuario responsable del restock
            var admin = new ApplicationUser()
            {
                Id = "1",
                UserName = "admin",
                Surname = "responsable",
                Email = "admin@test.com",
            };
            
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

            // Crear restock con items
            var restock = new Restock()
            {
                Title = "Restock Test",
                DeliveryAddress = "Warehouse A",
                Description = "Restocking items",
                ExpectedDate = DateTime.Today.AddDays(2),
                RestockDate = DateTime.Today.AddDays(5),
                TotalPrice = 0,
                RestockResponsible = admin,
                RestockItems = new List<RestockItem>()
                {
                    new RestockItem(){ Item = items[0], Quantity = 10, RestockPrice = 5 },
                    new RestockItem(){ Item = items[1], Quantity = 20, RestockPrice = 10 },
                }
            };

            _context.Users.Add(admin);
            _context.Items.AddRange(items);
            _context.Brands.AddRange(brands);
            _context.ItemTypes.AddRange(itemTypes);
            _context.Restock.Add(restock);

            _context.SaveChanges();



        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRestockDetails_NotFound_Test()
        {
            var mock = new Mock<ILogger<RestockController>>();
            ILogger<RestockController> logger = mock.Object;

            var controller = new RestockController(_context, logger);

            var result = await controller.GetRestockDetails(999);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRestockDetails_Successful_Test()
        {
            var mock = new Mock<ILogger<RestockController>>();
            ILogger<RestockController> logger = mock.Object;
            var controller = new RestockController(_context, logger);


            var expectedRestock = new RestockDetailDTO(
                id: 1,
                title: "Restock Test",
                deliveryAddress: "Warehouse A",
                description: "Restocking items",
                expectedDate: DateTime.Today.AddDays(2),
                restockDate: DateTime.Today.AddDays(5),
                totalPrice: 0,
                restockItems: new List<RestockItemForCreateDTO>()
                {
                    new RestockItemForCreateDTO("Foam Roller", 1, 10, 5),
                    new RestockItemForCreateDTO("Bands", 2, 20, 10)
                },
                restockResponsible: "admin",
                adminSurname: "responsable"
            );

            //Act
            var result = await controller.GetRestockDetails(1);


            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var restockDTOActual = Assert.IsType<RestockDetailDTO>(okResult.Value);
            var restockDTOExpected = expectedRestock.Equals(restockDTOActual);

            Assert.Equal(expectedRestock.Id, restockDTOActual.Id);
            Assert.Equal(expectedRestock.Title, restockDTOActual.Title);
            Assert.Equal(expectedRestock.DeliveryAddress, restockDTOActual.DeliveryAddress);
            Assert.Equal(expectedRestock.Description, restockDTOActual.Description);
            Assert.Equal(expectedRestock.TotalPrice, restockDTOActual.TotalPrice);
            Assert.Equal(expectedRestock.RestockResponsible, restockDTOActual.RestockResponsible);
            Assert.Equal(expectedRestock.AdminSurname, restockDTOActual.AdminSurname);

            // Comparar items
            Assert.Equal(expectedRestock.RestockItems.Count, restockDTOActual.RestockItems.Count);
            for (int i = 0; i < expectedRestock.RestockItems.Count; i++)
            {
                Assert.Equal(expectedRestock.RestockItems[i].ItemName, restockDTOActual.RestockItems[i].ItemName);
                Assert.Equal(expectedRestock.RestockItems[i].Quantity, restockDTOActual.RestockItems[i].Quantity);
                Assert.Equal(expectedRestock.RestockItems[i].RestockPrice, restockDTOActual.RestockItems[i].RestockPrice);
            }


        }
    }
}
