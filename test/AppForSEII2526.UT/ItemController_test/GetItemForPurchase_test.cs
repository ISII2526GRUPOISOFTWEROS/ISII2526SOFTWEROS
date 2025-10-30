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
                new Brand(){ Id=1, Name="Nike"},
                new Brand(){ Id=2, Name="Joma"},

                // PONER EXCEPCIONES PARA QUE NO SALGA RESULTADO (WHERE)
            };
            var items = new List<Item>()
            {
                new Item(){ Id=1, Name="Foam Roller", Brand=brands[0], Description="Description1", PurchasePrice=10.0m, QuantityAvailableForPurchase=100},
                new Item(){ Id=2, Name="Bands", Brand=brands[1], Description="Description2", PurchasePrice=20.0m, QuantityAvailableForPurchase=200},
                new Item(){ Id=3, Name="Kettlebell", Brand=brands[0], Description="Description3", PurchasePrice=30.0m, QuantityAvailableForPurchase=300},
                
                // PONER EXCEPCIONES PARA QUE NO SALGA RESULTADO (WHERE)

            };
   
            _context.Brands.AddRange(brands);
            _context.Items.AddRange(items);
            _context.SaveChanges();

        }

        [Fact]
        [Trait("GetItemForPurchase", "Unit Testing")]
        public async Task GetItemForPurchaseNull4ItemBrand_test()
        {
            List<ItemForPurchaseDTO> expectedItems = new List<ItemForPurchaseDTO>()
            {
                new ItemForPurchaseDTO(1, "Foam Roller", "Nike", "Description1", 10.0m, 100),
                new ItemForPurchaseDTO(2, "Bands", "Joma", "Description2", 20.0m, 200),
                new ItemForPurchaseDTO(3, "Kettlebell", "Nike", "Description3", 30.0m, 300),
            };
            var mock = new Mock<ILogger<ItemsController>>();
            ILogger<ItemsController> logger = mock.Object;
            ItemsController controller = new ItemsController(_context, null);

            //act
            var result = await controller.GetItemsForPurchase(null, null);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var itemactualresult = Assert.IsType<List<ItemForPurchaseDTO>>(okResult.Value);
            Assert.Equal(expectedItems, itemactualresult);
        }

    }
}
