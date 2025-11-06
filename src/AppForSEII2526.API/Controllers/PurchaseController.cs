using AppForSEII2526.API.DTOs.ItemDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

