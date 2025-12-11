using AppForSEII2526.API.DTOs.ItemDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace AppForSEII2526.API.Controllers
{

    //Restock Get Method
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {

        private ApplicationDbContext _context; //Access to the db
        private ILogger<ItemsController> _logger;

        public ItemsController(ApplicationDbContext context, ILogger<ItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ItemForRestockDTO>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
            
        public async Task<ActionResult> GetItemsForRestock(string? itemName, int? quantityForRestock)
        {
            IList<ItemForRestockDTO> itemsDTOs = await _context.Items
                .Where(Item => (itemName == null ||  Item.Name.Contains(itemName))
                            
                            && (quantityForRestock == null || Item.QuantityAvailableForPurchase <= quantityForRestock))


                .OrderBy(Item => Item.Name)

                .Select(Item => new ItemForRestockDTO(
                                                    Item.Id, 
                                                    Item.Brand.Name,
                                                    Item.Name ?? string.Empty,
                                                    Item.QuantityAvailableForPurchase,
                                                    Item.QuantityForRestock))

                .ToListAsync();

            if (itemsDTOs.Count == 0)
            {
                string error = "The item must need a restock";
                _logger.LogWarning(DateTime.Now + " " + error);
                return BadRequest(error);
            }

            return Ok(itemsDTOs);

        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ItemForPurchaseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetItemsForPurchase(string? itemName, string? itemBrand)
        {
            IList<ItemForPurchaseDTO> itemsDTOS = await _context.Items
                .Include(i => i.Brand)
                .Where(i => (itemName == null || i.Name.Contains(itemName)) && (itemBrand == null || i.Brand.Name.Contains(itemBrand)))
                .OrderBy(i => i.Id)
                .Select(item => new ItemForPurchaseDTO(item.Id, item.Name ?? string.Empty, item.Brand.Name ?? string.Empty, item.Description ?? string.Empty, item.PurchasePrice, item.QuantityAvailableForPurchase))
                .ToListAsync();

            if (itemsDTOS.Count == 0)
            {
                string error = "No items found for the given criteria.";
                _logger.LogWarning(DateTime.Now + " " + error);
                return BadRequest(error);
            }
            return Ok(itemsDTOS);
        }   
    }
}