using AppForSEII2526.API.DTOs.ItemDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace AppForSEII2526.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CreateRestockController : ControllerBase
    {
            private ApplicationDbContext _context; //Access to the db
            private ILogger<CreateRestockController> _logger;

            public CreateRestockController(ApplicationDbContext context, ILogger<CreateRestockController> logger)
            {
                _context = context;
                _logger = logger;
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
                    .FirstOrDefaultAsync(user => user.UserName == restockForCreate.RestockResponsible);
                if (admin == null)
                {
                    return Conflict("El responsable de reposición no existe.");
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
                    restockForCreate.RestockItems.Select(ri => new RestockItem
                    {
                        ItemId = ri.ItemId,
                        Quantity = ri.Quantity,
                        RestockPrice = ri.RestockPrice
                    }).ToList(),
                    admin
                );  



                foreach (var restockItem in restockForCreate.RestockItems)
                {
                    var existingItem = items.FirstOrDefault(i => i.Name == restockItem.ItemName);

                    if (existingItem == null)
                    {
                        ModelState.AddModelError("RestockItems", $"El ítem con nombre {restockItem.ItemName} no existe.");
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
                        _logger.LogError(DateTime.Now+" : "+ ex.Message);
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
                    restock.RestockItems new RestockItemForCreateDTO()
                    {
                        ItemId = restock.Id,
                        ItemName = restock.Title ?? "(Unknown)",
                        Quantity = restock.Id,
                        RestockPrice = restock.TotalPrice
                    },
                    admin.UserName
                    );


                return CreatedAtAction("GetRestock",new { id = restock.Id }, restockForCreate);
            }


        


    }
}
