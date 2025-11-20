using AppForSEII2526.API.DTOs.RestockDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Linq;

namespace AppForSEII2526.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RestockController : ControllerBase
    {
            private ApplicationDbContext _context; //Access to the db
            private ILogger<RestockController> _logger;

            public RestockController(ApplicationDbContext context, ILogger<RestockController> logger)
            {
                _context = context;
                _logger = logger;
            }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(RestockDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> GetRestockDetails(int id)
        {
            if(_context.Restock == null)
            {
                _logger.LogError("Error: Restock table does not exist");
                return NotFound();
            }


            var restock = await _context.Restock
                .Where(r => r.Id == id)
                .Include(r => r.RestockItems)
                    .ThenInclude(ri => ri.Item)
                .Include(ru => ru.RestockResponsible)
                .Select(r => new RestockDetailDTO(
                    r.Id,
                    r.Title,
                    r.DeliveryAddress,
                    r.Description,
                    r.ExpectedDate,
                    r.RestockDate,
                    r.TotalPrice,
                    r.RestockItems
                        .Select(ri => new RestockItemForCreateDTO(ri.Item.Name, 
                        ri.Item.Id, 
                        ri.Quantity, 
                        ri.RestockPrice)).ToList<RestockItemForCreateDTO>()
                        , r.RestockResponsible.UserName,
                    r.RestockResponsible.Surname))
                .FirstOrDefaultAsync();


            if (restock == null)
            {
                return NotFound();
            }

            return Ok(restock);
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
                if (restockForCreate.RestockItems == null || restockForCreate.RestockItems.Count == 0)
                {
                    ModelState.AddModelError("RestockItems", "Error! At least one restock item is required.");
                }
                if(!string.IsNullOrEmpty(restockForCreate.Description) && 
                   !restockForCreate.Description.StartsWith("Restock for"))
                {
                //return BadRequest("Error! You must start the Description with 'Restock for'");
                   ModelState.AddModelError("Description", "Error! You must start the Description with 'Restock for'");
            }
                


            //if (!ModelState.IsValid)
            //{
            //    return ValidationProblem(ModelState);
            //}

            // Verificar que el usuario responsable de la reposición existe
            var admin = await _context.Users
                    .FirstOrDefaultAsync(user => user.UserName == restockForCreate.RestockResponsible);
                if (admin == null)
                {
                    ModelState.AddModelError("RestockResponsible", "The user responsible for the restock does not exist.");
            }

                if (ModelState.ErrorCount > 0)
                {
                    return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Obtenemos los Items de la base de datos y verificamos su disponibilidad para reposición
            var itemsNames = restockForCreate.RestockItems.Select(ri => ri.ItemName).ToList<string>();

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
                    new List<RestockItem>(),
                    admin
                );  



                foreach (var restockItem in restockForCreate.RestockItems)
                {
                    var existingItem = items.FirstOrDefault(i => i.Name == restockItem.ItemName);

                    if (existingItem == null || existingItem.QuantityForRestock < restockItem.Quantity)
                    {
                        ModelState.AddModelError("RestockItems", $"The Item with name {restockItem.ItemName} is not available for restock");
                    }

                else
                {
                        restock.RestockItems.Add(new RestockItem
                        {
                            ItemId = existingItem.Id,
                            Quantity = restockItem.Quantity,
                            RestockPrice = restockItem.RestockPrice

                        });
                    }
                }


            //Total price calculation
            restock.TotalPrice = restock.RestockItems.Sum(ri => ri.Quantity * ri.RestockPrice);

            //looking for model errors
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
                        _logger.LogError(DateTime.Now+" : "+ ex.Message);
                    ModelState.AddModelError("Restock", "An error occurred while saving the restock. Please try again later.");
                        return Conflict("An error occurred"+ ex.Message);
                    }
                
                var restockDetail = new ItemForCreateRestockDTO(
                    restock.Id,
                    restock.Title,
                    restock.DeliveryAddress,
                    restock.Description,
                    restock.ExpectedDate,
                    restock.RestockDate,
                    restock.TotalPrice,
                    restockForCreate.RestockItems,
                    admin.UserName
                    );


                return CreatedAtAction("GetRestock",new { id = restock.Id }, restockForCreate);
            }


        


    }
}
