using AppForSEII2526.API.DTOs.ItemDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {

        private ApplicationDbContext _context;
        private ILogger<ItemsController> _logger;

        public ItemsController(ApplicationDbContext context, ILogger<ItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]

        //public async Task <ActionResult> ComputeDivision(decimal op1, decimal op2)
        //{
        //    if(op2 == 0)
        //    {
        //        string error ="Can't divide by zero";
        //        _logger.LogError(DateTime.Now+error);
        //        return BadRequest(error);
        //    }

        //    decimal result = op1 / op2;
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ItemForRestockDTO>), (int) HttpStatusCode.OK)]
            
        public async Task<ActionResult> GetItemsForRestock(string? itemName, int? quantityForRestock)
        {
            IList<ItemForRestockDTO> itemsDTOs = await _context.Items
                .Where(Item => Item.Name.Contains(itemName) 
                            || Item.QuantityForRestock > quantityForRestock)

                .OrderBy(Item => Item.Name)

                .Select(Item => new ItemForRestockDTO(Item.Id, Item.Name, Item.Brand.Name))

                .ToListAsync();

            return Ok(itemsDTOs);

        }

    }
}
