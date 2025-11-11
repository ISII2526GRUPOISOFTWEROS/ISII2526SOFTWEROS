using AppForSEII2526.API.DTOs.ItemDTOs;

namespace AppForSEII2526.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ItemGetRestock: ControllerBase
    {

            private ApplicationDbContext _context; //Access to the db
            private ILogger<ItemGetRestock> _logger;

            public ItemGetRestock(ApplicationDbContext context, ILogger<ItemGetRestock> logger)
            {
                _context = context;
                _logger = logger;
            }



            [HttpGet]
            [Route("[action]")]
            [ProducesResponseType(typeof(IList<ItemForRestockDTO>), (int)HttpStatusCode.OK)]
            [ProducesResponseType((int)HttpStatusCode.NotFound)]

            public async Task<ActionResult> GetItemsForRestock(string? itemName, int? quantityForRestock)
            {
                IList<ItemForRestockDTO> itemsDTOs = await _context.Items
                    .Where(Item => Item.Name.Contains(itemName)
                                || Item.QuantityForRestock > quantityForRestock)

                    .OrderBy(Item => Item.Name)

                    .Select(Item => new ItemForRestockDTO(
                                                        Item.Brand.Name,
                                                        Item.Name ?? string.Empty,
                                                        Item.QuantityAvailableForPurchase,
                                                        Item.QuantityForRestock))

                    .ToListAsync();

                return Ok(itemsDTOs);

            }
        
    }
}
