using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ItemDTOs;
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
                new Item(){ Name="ItemA", QuantityForRestock=30, Brand= brands[0], QuantityAvailableForPurchase=25 ,ItemType = itemTypes[0]},
                new Item(){ Name="ItemB", QuantityForRestock=15, Brand=brands[1], QuantityAvailableForPurchase=20 ,ItemType = itemTypes[1]},
                new Item(){ Name="ItemC", QuantityForRestock=25, Brand=brands[0], QuantityAvailableForPurchase=30 ,ItemType = itemTypes[0]},

            };


            _context.Items.AddRange(items);
            _context.Brands.AddRange(brands);
            _context.ItemTypes.AddRange(itemTypes);

            _context.SaveChanges();

            //var itemsDTOsTc1 = new List<ItemForCreateRestockDTO>() { items[0], items[1] };



        }
        [Fact]
        public async Task GetItemsForRestock_ShouldReturnItemsMatchingCriteria()
        {
            // Arrange
            List<ItemForRestockDTO> expectedItems = new List<ItemForRestockDTO>()
            {
                new ItemForRestockDTO("Nike", "ItemA", 25, 30)
            };
            var mock = new Mock<ILogger<ItemsController>>();
            ILogger<ItemsController> logger = mock.Object;
            ItemsController controller = new ItemsController(_context, logger);

            // Act
            var result = await controller.GetItemsForRestock("ItemA", null);

            // Assert
            var okresult = Assert.IsType<OkObjectResult>(result);
            var itemactualresult = Assert.IsType<List<ItemForRestockDTO>>(okresult.Value);
            Assert.Equal(expectedItems, itemactualresult);
        }

        //Theory made in class
        //[Theory]
        //[Trait("LevelTesting", "Unit testing")]
        //[MemberData(nameof(GetItemsForRestock_test))]
        //public async Task GetItemsForRestock_ShouldReturnItemsMatchingCriteria(string? itemName, int? quantityForRestock)
        //{
        //    // Arrange
        //    var controller = new ItemsController(_context, null);

        //    // Act
        //    var result = await controller.GetItemsForRestock(itemName, quantityForRestock);



        //    // Assert
        //    Assert.NotNull(result);
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var items = Assert.IsAssignableFrom<IList<ItemForRestockDTO>>(okResult.Value);
        //    Assert.Single(items);
        //    Assert.Equal("ItemA", items[0].Name);
        //}


    }
}
