using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
//using AppForSEII2526.DTOs

namespace AppForSEII2526.UT.ItemController
{
    public class GetItemsForRestock_test : AppForSEII25264SqliteUT
    {
        public GetItemsForRestock_test() //constructor
        {
            var items = new List<Item>()
            {   
                new Item(){ Id=1, Name="ItemA", QuantityForRestock=5, Brand=new Brand(){ Name="BrandA"}, QuantityAvailableForPurchase=10 },
                new Item(){ Id=2, Name="ItemB", QuantityForRestock=15, Brand=new Brand(){ Name="BrandB"}, QuantityAvailableForPurchase=20 },
                new Item(){ Id=3, Name="ItemC", QuantityForRestock=25, Brand=new Brand(){ Name="BrandC"}, QuantityAvailableForPurchase=30 },

            };

            

        }
        [Fact]
        public async Task GetItemsForRestock_ShouldReturnItemsMatchingCriteria()
        {
            // Arrange
            var controller = new ItemsController(_context, _logger);
            string itemName = "ItemA";
            int quantityForRestock = 10;
            // Act
            var result = await controller.GetItemsForRestock(itemName, quantityForRestock) as ActionResult;
            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var items = Assert.IsAssignableFrom<IList<ItemForRestockDTO>>(okResult.Value);
            Assert.Single(items);
            Assert.Equal("ItemA", items[0].Name);
        }


        }
}
