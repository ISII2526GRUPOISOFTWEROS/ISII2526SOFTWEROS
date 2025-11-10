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
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ItemForCreateRestockDTO), (int)HttpStatusCode.Created)]


        public async Task<ActionResult> CreateRestock(ItemForCreateRestockDTO restockForCreate)
        {

            // Validar las condiciones obligatorias iniciales
            if (string.IsNullOrEmpty(restockForCreate.Title))
            {
                ModelState.AddModelError("Title", "Error! Title is required.");
            }

            if (string.IsNullOrEmpty(restockForCreate.DeliveryAddress))
            {
                ModelState.AddModelError("DeliveryAddress", "Error! Delivery Address is required.");
            }
            if (restockForCreate.RestockItems.Count == 0)
            {
                ModelState.AddModelError("RestockItems", "Error! At least one restock item is required.");
            }


            //if (!ModelState.IsValid)
            //{
            //    return ValidationProblem(ModelState);
            //}

            // Verificar que el usuario responsable de la reposición existe
            var admin = await _context.Users
                .FirstOrDefaultAsync(user => user.UserName == restockForCreate.RestockResponsible.Name);
            if (admin == null)
            {
                return Conflict("El responsable de reposición no existe.");
            }

            // Obtenemos los Items de la base de datos y verificamos su disponibilidad para reposición
            var itemsNames = restockForCreate.RestockItems.Select(ri => ri.Item.Name).ToList<string>();

            var items = _context.Items.Include(i => i.RestockItems)
                .ThenInclude(ri => ri.Restock)

                .Where(i => itemsNames.Contains(i.Name))

                .Select(i => new {
                    i.Id,
                    i.Name,
                    i.QuantityForRestock
                })
                .ToList();


            //crear la entidad de restock
            Restock restock = new Restock(
                restockForCreate.DeliveryAddress,
                restockForCreate.Description,
                restockForCreate.ExpectedDate,
                restockForCreate.Id,
                restockForCreate.RestockDate,
                restockForCreate.Title,
                restockForCreate.TotalPrice,
                restockForCreate.RestockItems,
                admin
            );



            foreach (var restockItem in restockForCreate.RestockItems)
            {
                var existingItem = items.FirstOrDefault(i => i.Name == restockItem.Item.Name);

                if (existingItem == null)
                {
                    ModelState.AddModelError("RestockItems", $"El ítem con nombre {restockItem.Item.Name} no existe.");
                }
                if (existingItem.QuantityForRestock < restockItem.Quantity)
                {
                    ModelState.AddModelError("RestockItems", $"No hay suficiente cantidad disponible para reposición del ítem con ID {restockItem.ItemId}.");
                }
                //asegurarse que item no este vacio
                if (restockForCreate.RestockItems == null || !restockForCreate.RestockItems.Any())
                {
                    ModelState.AddModelError("RestockItems", "Debe especificar al menos un ítem para reposición.");
                }

                else
                {
                    restock.RestockItems.Add(new RestockItem
                    {
                        ItemId = existingItem.Id,
                        Quantity = restockItem.Quantity,
                        RestockId = restock.Id,
                        RestockPrice = restockItem.RestockPrice,
                        Restock = restock,
                        Item = null! // Asignar el ítem correspondiente si es necesario

                    });
                }
            }


            //Calcular el precio total del restock
            restock.TotalPrice = restock.RestockItems.Sum(ri => ri.Quantity * ri.RestockPrice);

            //verificacion de errores
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }


            _context.Restock.Add(restock);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(DateTime.Now + " : " + ex.Message);
                return Conflict("An error occurred" + ex.Message);
            }

            var restockDetail = new ItemForCreateRestockDTO(
                restock.Id,
                restock.Title,
                restock.DeliveryAddress,
                restock.Description,
                restock.ExpectedDate,
                restock.RestockDate,
                restock.TotalPrice,
                TODO, TODO);


            return CreatedAtAction("GetRestock", new { id = restock.Id }, restockForCreate);
        }





    }

   
}

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
        


