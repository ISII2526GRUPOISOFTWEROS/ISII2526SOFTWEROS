using AppForSEII2526.API.DTOs.ItemDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
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

        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal),(int)HttpStatusCode.OK)]//Successful return
        //[ProducesResponseType(typeof(string),(int)HttpStatusCode.BadRequest)]//Bad return
        //public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        //{
        //    if(op2== 0)
        //    {
        //        string error ="Division by zero is not allowed.";
        //       //_logger.LogError(DateTime.Now+   error);
        //        return BadRequest(error);
        //    }
        //    decimal result = op1/ op2;
        //    return Ok(result);
        //}
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ItemForPurchaseDTO>),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetItemsForPurchase(string? itemName, string? itemBrand)
        {
            IList<ItemForPurchaseDTO> itemsDTOS = await _context.Items
                .Include(i=>i.Brand)
                .Where(i=>  (itemName==null || i.Name.Contains(itemName)) && (itemBrand == null || i.Brand.Name.Contains(itemBrand)) )
                .OrderBy(i=>i.Name)
                .Select(item=>new ItemForPurchaseDTO(item.Id, item.Name ?? string.Empty, item.Brand.Name ?? string.Empty, item.Description ?? string.Empty, item.PurchasePrice, item.QuantityAvailableForPurchase))
                .ToListAsync();

            if(itemsDTOS.Count == 0)
            {
                string error = "No items found for the given criteria.";
                _logger.LogWarning(DateTime.Now + " " + error);
                return BadRequest(error);
            }
            return Ok(itemsDTOS);
        }
    }   
}