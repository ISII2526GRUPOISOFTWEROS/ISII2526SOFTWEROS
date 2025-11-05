using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.Controllers;

namespace AppForSEII2526.UT.ItemController_test
{
    public class PostItemFortPurchase_test : AppForSEII25264SqliteUT
    {

        public PostItemFortPurchase_test()
        {
            ApplicationUser user = new ApplicationUser(
                "1",
                "Pepe",
                "Perez",
                "pepegomez@gmail.com",
                "Calle Falsa 123"
                );

            _context.ApplicationUser.Add(user);
        }

        [Fact]
        [Trait("CreateItemForPurchase", "Unit Testing")]
        public async Task PostItemForPurchaseNull4ItemBrand_test()
        {
            var expectedItems = new List<ItemForCreateDTO>()
            {
           
            };

            var mock = new Mock<ILogger<ItemsController>>();
            ILogger<ItemsController> logger = mock.Object;
            ItemsController controller = new ItemsController(_context, logger);

            //act
            var result = await controller.CreateItemForPurchase(null);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var itemactualresult = Assert.IsType<List<ItemForPurchaseDTO>>(okResult.Value);



            Assert.Equal(expetedItemsSorted, itemactualresultSorted);
        }
    }
}