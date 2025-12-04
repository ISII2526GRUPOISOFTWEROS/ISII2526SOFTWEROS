using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private ApplicationDbContext _context; //Access to the db
        private ILogger<PurchaseController> _logger;

        public PurchaseController(ApplicationDbContext context, ILogger<PurchaseController> logger)
        {
            _context = context;
            _logger = logger;
        }
        private static string GetPaymentMethodName(PaymentMethod? paymentMethod)
        {
            return paymentMethod switch
            {
                CreditCard => "Credit Card",
                Bizum => "Bizum",
                PayPal => "PayPal",
                _ => "Unknown"
            };
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateItemForPurchase(ItemForCreateDTO itemForCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ValidationProblem(ModelState));
            }


            var user = _context.ApplicationUser.FirstOrDefault(au => au.UserName == itemForCreate.CustomerUserName || au.Email == itemForCreate.CustomerUserName );
            if (user == null)
            {
                ModelState.AddModelError("UserNotFound", $"Error! Username or email is not registred.");
                return BadRequest(ValidationProblem(ModelState));
            }
            //var checkPM = await _context.Set<PaymentMethod>()
            //    .AnyAsync(pm => pm.Id == itemForCreate.PaymentMethodId && pm.User.Id == user.Id);


            //if (!checkPM)
            //{
            //    ModelState.AddModelError("PaymentMethod", "Error! The selected payment method is not registered for this user.");
            //    return BadRequest(ValidationProblem(ModelState));

            //}

            var paymentMethod = await _context.Set<PaymentMethod>().FirstOrDefaultAsync(pm => pm.Id == itemForCreate.PaymentMethodId && pm.User.Id == user.Id);

            if (paymentMethod == null || paymentMethod.User.Id != user.Id)
            {
                ModelState.AddModelError("PaymentMethod", "Error! The selected payment method is not registered for this user.");
                return BadRequest(ValidationProblem(ModelState));
            }
            string sentence = "My purchase for";

            if (itemForCreate.Description != "" && !itemForCreate.Description.StartsWith(sentence)) {
                ModelState.AddModelError("Description", "Error! You must start the Description with My purchase for.");
                return BadRequest(ValidationProblem(ModelState));
            }

            
            var requestedItemsIds = itemForCreate.PurchaseItems.Select(pi => pi.ItemId).ToList();

            var dbItems = await _context.Items
                .Where(i => requestedItemsIds.Contains(i.Id))
                .Include(i=>i.Brand)
                .ToDictionaryAsync(i => i.Id);

            decimal totalCost = 0;
            var purchaseItems = new List<PurchaseItem>();

            foreach (var pi in itemForCreate.PurchaseItems)
            {
                if (!dbItems.TryGetValue(pi.ItemId, out var dbItem))
                {
                    ModelState.AddModelError("ItemNotFound", $"Error! Item with Id {pi.ItemId} not found.");
                    continue;
                }
                if (pi.Quantity <= 0)
                {
                    ModelState.AddModelError("InvalidQuantity", $"Error! Item {dbItem.Name} has invalid quantity {pi.Quantity}. Quantity must be greater than zero.");
                    continue;
                }

                if (pi.Quantity > dbItem.QuantityAvailableForPurchase)
                {
                    ModelState.AddModelError("InsufficientStock", $"Error! Item {dbItem.Name} does not have enough stock. Available: {dbItem.QuantityAvailableForPurchase}, Requested: {pi.Quantity}");
                    continue;
                }
                purchaseItems.Add(new PurchaseItem
                {
                    ItemId = dbItem.Id,
                    Amount_bought = pi.Quantity,
                    Price = dbItem.PurchasePrice
                });

                totalCost += dbItem.PurchasePrice * pi.Quantity;
                dbItem.QuantityAvailableForPurchase -= pi.Quantity;
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

            var result = new PurchaseDetailDTO(
                purchase.Id,
                GetPaymentMethodName(purchase.PaymentMethod),
                purchase.Street,
                purchase.City,
                purchase.Country,
                purchase.Description ?? string.Empty,
                purchase.PurchaseItems.Select(pi => {
                    var item = dbItems[pi.ItemId];

                    return new PurchasedItemDTO(
                        item.Name ?? string.Empty,
                        item.Brand.Name ?? string.Empty,
                        pi.Price,
                        pi.Amount_bought
                    );
                }).ToList(),
                purchase.Total_prices);
   
            return CreatedAtAction("GetPurchaseDetails", new { id = purchase.Id }, result);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PurchaseDetailDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPurchaseDetails(int id)
        {
            if (_context.Purchases == null)
            {
                _logger.LogError(DateTime.Now + " Purchases table does not exist.");
                return NotFound();
            }

            IList<PurchaseDetailDTO> purchaseDetails = await _context.Purchases
                .Where(p => p.Id == id)
                .Include(p => p.PaymentMethod)
                .Include(p => p.PurchaseItems)
                .ThenInclude(pi => pi.Item)
                .ThenInclude(pi => pi.Brand)
                .Select(p => new PurchaseDetailDTO(
                    p.Id,
                    GetPaymentMethodName(p.PaymentMethod),
                    p.Street,
                    p.City,
                    p.Country,
                    p.Description ?? string.Empty,
                    p.PurchaseItems.Select(pi => new PurchasedItemDTO(
                        pi.Item.Name ?? string.Empty,
                        pi.Item.Brand.Name ?? string.Empty,
                        pi.Price,
                        pi.Amount_bought)).ToList(),
                    p.Total_prices))
                .ToListAsync();

            if (purchaseDetails == null || !purchaseDetails.Any())
            {
                _logger.LogError(DateTime.Now + $" Rental with id {id} dot not exist.");
                return NotFound();
            }
            return Ok(purchaseDetails);
        }
    }
}

