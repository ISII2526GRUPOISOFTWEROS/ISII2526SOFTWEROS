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
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ItemForPurchaseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateItemForPurchase(ItemForCreateDTO itemForCreate)
        {
            var user = _context.ApplicationUser.FirstOrDefault(au => au.UserName == itemForCreate.CustomerUserName);

            if (user == null)
            {
                ModelState.AddModelError("UserNotFound", $"Error! Username is not registred");
            }
            if(user!= null && !user.PaymentMethods.Any(pm => pm.Id == itemForCreate.PaymentMethodId))
            {
                ModelState.AddModelError("PaymentMethod", "Error! The selected payment method is not registered for this user.");
            }
            
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ValidationProblem(ModelState));
            }
       
            var reqiestedItemsIds = itemForCreate.PurchaseItems.Select(pi => pi.Id).ToList();
            var dbItems = await _context.Items
                .Where(i => reqiestedItemsIds.Contains(i.Id))
                .ToDictionaryAsync(i=>i.Id);

            decimal totalCost = 0;

            foreach(var purchaseItem in itemForCreate.PurchaseItems)
            {
                if(!dbItems.TryGetValue(purchaseItem.Id, out var dbItem))
                {
                    ModelState.AddModelError("ItemNotFound", $"Error! Item with Id {purchaseItem.Id} not found.");
                    continue;
                }
                var quantityToPurchase = purchaseItem.QuantityAvailableForPurchase;
                if (quantityToPurchase <= 0) {
                    ModelState.AddModelError("InvalidQuantity", $"Error! Item {dbItem.Name} has invalid quantity {quantityToPurchase}. Quantity must be greater than zero.");
                    continue;
                }

                if (quantityToPurchase > purchaseItem.QuantityAvailableForPurchase)
                {
                    ModelState.AddModelError("InsufficientStock", $"Error! Item {dbItem.Name} does not have enough stock. Available: {dbItem.QuantityAvailableForPurchase}, Requested: {purchaseItem.QuantityAvailableForPurchase}");
                    continue;
                }
                totalCost += dbItem.PurchasePrice * quantityToPurchase;
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ValidationProblem(ModelState));
            }
            var itemforcreate = new ItemForCreateDTO
            {
                CustomerUserName = itemForCreate.CustomerUserName,
                PaymentMethodId = itemForCreate.PaymentMethodId,
                Street = itemForCreate.Street,
                City = itemForCreate.City,
                Country = itemForCreate.Country,
                Description = itemForCreate.Description,
                PurchaseItems = itemForCreate.PurchaseItems,
                TotalPrice = totalCost
            };
            foreach(var purchaseItem in itemForCreate.PurchaseItems)
            {
                var dbItem = dbItems[purchaseItem.Id];
                dbItem.QuantityAvailableForPurchase -= purchaseItem.QuantityAvailableForPurchase;
                _context.Items.Update(dbItem);

            }

            _context.Items.Add(itemforcreate);
            try
            {
                await _context.SaveChangesAsync();

            } catch(Exception ex) {
                _logger.LogError(DateTime.Now + " " + ex.Message);
                ModelState.AddModelError("Item", $"Error! There was an error while saving your item, plese, try again later");
                return Conflict("Error" + ex.Message);
            }

            return CreatedAtAction("GetItem", new {id= .Id},);
        }
    }   
}