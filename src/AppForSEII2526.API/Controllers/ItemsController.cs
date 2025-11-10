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
        [ProducesResponseType(typeof(IList<ItemForRestockDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> GetItemsForRestock(string? itemName, int? quantityForRestock)
        {
            IList<ItemForRestockDTO> itemsDTOs = await _context.Items
                .Where(Item => Item.Name.Contains(itemName)
                            || Item.QuantityForRestock > quantityForRestock)

                .OrderBy(Item => Item.Name)

                .Select(Item => new ItemForRestockDTO(Item.Id,
                                                    Item.Name,
                                                    Item.Brand.Name,
                                                    Item.QuantityAvailableForPurchase,
                                                    Item.QuantityForRestock))

                .ToListAsync();

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
                .OrderBy(i => i.Name)
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




        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ItemForPurchaseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateItemForPurchase(ItemForCreateDTO itemForCreate)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ValidationProblem(ModelState));
            }


            var user = _context.ApplicationUser.FirstOrDefault(au => au.UserName == itemForCreate.CustomerUserName);

            if (user == null)
            {
                ModelState.AddModelError("UserNotFound", $"Error! Username is not registred");
                return BadRequest(ValidationProblem(ModelState));
            }
            var checkPM = await _context.Set<PaymentMethod>()
                .AnyAsync(pm => pm.Id == itemForCreate.PaymentMethodId && pm.User.Id == user.Id);


            if (!checkPM)
            {
                ModelState.AddModelError("PaymentMethod", "Error! The selected payment method is not registered for this user.");
                return BadRequest(ValidationProblem(ModelState));

            }

            var paymentMethod = await _context.Set<PaymentMethod>().FirstOrDefaultAsync(pm => pm.Id == itemForCreate.PaymentMethodId);

            if (paymentMethod == null || paymentMethod.User.Id != user.Id)
            {
                ModelState.AddModelError("PaymentMethod", "ERROR! The selected payment method is not registered for this user.");
                return BadRequest(ValidationProblem(ModelState));
            }

            var requestedItemsIds = itemForCreate.PurchaseItems.Select(pi => pi.Id).ToList();

            var dbItems = await _context.Items
                .Where(i => requestedItemsIds.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id);

            decimal totalCost = 0;
            var purchaseItems = new List<PurchaseItem>();

            foreach (var pi in itemForCreate.PurchaseItems)
            {
                if (!dbItems.TryGetValue(pi.Id, out var dbItem))
                {
                    ModelState.AddModelError("ItemNotFound", $"Error! Item with Id {pi.Id} not found.");
                    continue;
                }
                if (pi.QuantityAvailableForPurchase <= 0)
                {
                    ModelState.AddModelError("InvalidQuantity", $"Error! Item {dbItem.Name} has invalid quantity {pi.QuantityAvailableForPurchase}. Quantity must be greater than zero.");
                    continue;
                }

                if (pi.QuantityAvailableForPurchase > dbItem.QuantityAvailableForPurchase)
                {
                    ModelState.AddModelError("InsufficientStock", $"Error! Item {dbItem.Name} does not have enough stock. Available: {dbItem.QuantityAvailableForPurchase}, Requested: {pi.QuantityAvailableForPurchase}");
                    continue;
                }
                purchaseItems.Add(new PurchaseItem
                {
                    ItemId = dbItem.Id,
                    Amount_bought = pi.QuantityAvailableForPurchase,
                    Price = dbItem.PurchasePrice
                });

                totalCost += dbItem.PurchasePrice * pi.QuantityAvailableForPurchase;
                dbItem.QuantityAvailableForPurchase -= pi.QuantityAvailableForPurchase;
                _context.Items.Update(dbItem);
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ValidationProblem(ModelState));
            }


            var purchase = new Purchase
            {
                Street = itemForCreate.Street,
                City = itemForCreate.City,
                Country = itemForCreate.Country,
                Description = itemForCreate.Description,
                Total_prices = totalCost,
                Date = DateTime.UtcNow,
                PurchaseItems = purchaseItems,
                PaymentMethod = paymentMethod
            };


            _context.Purchases.Add(purchase);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(DateTime.Now + " " + ex.Message);
                ModelState.AddModelError("Item", $"Error! There was an error while saving your item, plese, try again later");
                return Conflict("Error" + ex.Message);
            }



            return CreatedAtAction("GetItemsForPurchase", new { id = purchase.Id }, new
            {
                purchase.Id,
                purchase.Total_prices,
                Items = purchaseItems.Select(pi => new
                {
                    pi.ItemId,
                    pi.Amount_bought,
                    pi.Price
                })
            });
        }

      

    }
}