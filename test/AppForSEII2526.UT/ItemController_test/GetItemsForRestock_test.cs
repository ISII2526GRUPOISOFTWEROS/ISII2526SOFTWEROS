using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.UT;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AppForSEII2526.DTOs

namespace AppForSEII2526.UT.RestockController_test
{
    public class GetItemsForRestock_test : AppForSEII25264SqliteUT
    {
        private ILogger<ItemsController> _logger;

        public GetItemsForRestock_test() //constructor  
        {
            var brands = new List<Brand>()
            {
                new Brand(){ Name="Nike"},
                new Brand(){ Name="Domyos"},

            };

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

            //var itemsDTOsTc1 = new List<ItemForCreateRestockDTO>() { items[0], items[1] };

        }


        public static IEnumerable<object[]> TestCasesFor_GetItemsForRestock_OK()
        {
            var itemDTOs = new List<ItemForRestockDTO>()
            {
                new ItemForRestockDTO(1, "Nike", "Foam Roller", 25, 30),
                new ItemForRestockDTO(2, "Domyos", "Bands", 15, 20),
                new ItemForRestockDTO(3, "Nike", "Kettlebell", 25, 30),
            };

            var itemDTOsTC1 = new List<ItemForRestockDTO>() { itemDTOs[1], itemDTOs[0], itemDTOs[2] };
            var itemDTOsTC2 = new List<ItemForRestockDTO>() { itemDTOs[0], itemDTOs[2] };
            var itemDTOsTC3 = new List<ItemForRestockDTO>() { itemDTOs[1] };
            var itemDTOsTC4 = new List<ItemForRestockDTO>() { itemDTOs[0] };



            var allTest = new List<object?[]>()
            {
                new object?[] { null, null, itemDTOsTC1 },
                new object?[] { "l", null,  itemDTOsTC2 },
                new object?[] { null, 20, itemDTOsTC3 },
                new object?[] { "Foam", null, itemDTOsTC4 },

                
            };
            return allTest;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetItemsForRestock_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit testing")]
        public async Task GetItemsForRestock_Ok_test(string? itemName, int? quantityForRestock, 
            IList<ItemForRestockDTO> expectedItems)
        {
            // Arrange
            var controller = new ItemsController(_context, null);

            // Act
            var result = await controller.GetItemsForRestock(itemName, quantityForRestock);

            

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var itemsDTOActual = Assert.IsType<List<ItemForRestockDTO>>(okResult.Value);
            Assert.Equal(expectedItems, itemsDTOActual);
        }



        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit testing")]
        public async Task GetItemsForRestock_badrequest_test()
        {
            // Arrange
            List<ItemForRestockDTO> expectedItems = new List<ItemForRestockDTO>()
            {
                new ItemForRestockDTO(1, "Nike", "Foam Roller", 30, 25)
            };
            var mock = new Mock<ILogger<ItemsController>>();
            ILogger<ItemsController> logger = mock.Object;
            ItemsController controller = new ItemsController(_context, logger);

            // Act
            var result = await controller.GetItemsForRestock("Foam Roller", 10);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problem = Assert.IsType<string>(badRequestResult.Value);


            Assert.Equal("The item must need a restock", problem);
        }

        
        


    }
}
