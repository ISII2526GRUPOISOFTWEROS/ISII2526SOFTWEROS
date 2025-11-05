using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.Controllers;

namespace AppForSEII2526.UT.ItemForPurchase_test 
{
    public class GetItemForPurchase_test : AppForSEII25264SqliteUT
    {
        public GetItemForPurchase_test()
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

            _context.Brands.AddRange(brands);
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
        public static IEnumerable<object[]> TestCasesFor_GetItemsForPurchase_OK()
        {
            var itemDTOs = new List<ItemForPurchaseDTO>()
            {
                new ItemForPurchaseDTO(1, "Foam Roller", "Nike", "Description1", 10.0m, 100),
                new ItemForPurchaseDTO(2, "Bands", "Domyos", "Description2", 20.0m, 200),
                new ItemForPurchaseDTO(3, "Kettlebell", "Nike", "Description3", 30.0m, 300),
            };
            
            var itemDTOsTC1 = new List<ItemForPurchaseDTO>() { itemDTOs[0], itemDTOs[1], itemDTOs[2] };
            var itemDTOsTC2 = new List<ItemForPurchaseDTO>() { itemDTOs[0], itemDTOs[2] };
            var itemDTOsTC3 = new List<ItemForPurchaseDTO>() { itemDTOs[1] };
            var itemDTOsTC4 = new List<ItemForPurchaseDTO>() { itemDTOs[1] };



            var allTest = new List<object?[]>()
            {
                new object?[] { null, null, itemDTOsTC1 },
                new object?[] { "l", null, itemDTOsTC2 },
                new object?[] { null, "Domyos", itemDTOsTC3 },
                new object?[] { "Foam", "Nike", itemDTOsTC3 },

            };
            return allTest;
        }

        [Fact]
        [Trait("GetItemForPurchase", "Unit Testing")]
        public async Task GetItemForPurchaseBadRequest__est()
        {
            var expectedItems = new List<ItemForPurchaseDTO>()
            {

            };
            var mock = new Mock<ILogger<ItemsController>>();
            ILogger<ItemsController> logger = mock.Object;
            ItemsController controller = new ItemsController(_context, logger);

            var result = await controller.GetItemsForPurchase("W", "Z");

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var itemactualresult = Assert.IsType<string>(badRequestResult.Value);


            Assert.Equal("No items found for the given criteria.", itemactualresult);
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetItemsForPurchase_OK))]
        public async Task GetItemForPurchaseFilter_test(string? itemName, string? brandName, List<ItemForPurchaseDTO> expectedItems)
        {
            var mock = new Mock<ILogger<ItemsController>>();
            ILogger<ItemsController> logger = mock.Object;
            ItemsController controller = new ItemsController(_context, logger);

            //act
            var result = await controller.GetItemsForPurchase(itemName, brandName);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var itemactualresult = Assert.IsType<List<ItemForPurchaseDTO>>(okResult.Value);

            var expetedItemsSorted = expectedItems.OrderBy(i => i.Name).ToList();
            var itemactualresultSorted = itemactualresult.OrderBy(i => i.Name).ToList();

            Assert.Equal(expetedItemsSorted, itemactualresultSorted);
        }
    }

}
