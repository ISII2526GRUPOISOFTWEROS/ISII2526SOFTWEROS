using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<PlanController> _logger;

        public PlanController(ApplicationDbContext context, ILogger<PlanController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PlanForCreateDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreatePlan(PlanForCreateDTO planForCreate)
        {
            if (planForCreate == null)
            {
                return BadRequest("No plan data provided.");
            }

            var user = _context.ApplicationUser.FirstOrDefault(au => au.UserName == planForCreate.UserName);

            if (user == null)
            {
                return BadRequest("Error! Username is not registered");
            }

            var checkPM = await _context.Set<PaymentMethod>()
                .AnyAsync(pm => pm.Id == planForCreate.PaymentMethodId && pm.User.Id == user.Id);


            if (!checkPM)
            {
                ModelState.AddModelError("PaymentMethod", "Error! The selected payment method is not registered for this user.");
                return BadRequest(ValidationProblem(ModelState));

            }



            if (string.IsNullOrWhiteSpace(planForCreate.Name))
            {
                ModelState.AddModelError("Name", "Plan name is mandatory.");
            }

            if (planForCreate.Weeks <= 0)
            {
                return BadRequest("Weeks must be greater than 0.");
            }

            if (planForCreate.PaymentMethodId <= 0)
            {
                ModelState.AddModelError("PaymentMethod", "A valid payment method must be selected.");
            }

            if (planForCreate.SelectedClasses == null || !planForCreate.SelectedClasses.Any())
            {
                return BadRequest("At least one class must be selected.");
            }
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ValidationProblem(ModelState));
            }

            var paymentMethod = await _context.Set<PaymentMethod>()
                .FirstOrDefaultAsync(pm => pm.Id == planForCreate.PaymentMethodId);

            if (paymentMethod == null)
            {
                ModelState.AddModelError("PaymentMethod", "The selected payment method is invalid or not found.");
                return BadRequest(ValidationProblem(ModelState));
            }

            var selectedClassIds = planForCreate.SelectedClasses.Select(c => c.Id).ToList();
            var dbClasses = await _context.Classes
                .Include(c => c.ItemType)
                .Where(c => selectedClassIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id);

            decimal totalCost = 0;
            var plan = new Plan
            {
                Name = planForCreate.Name,
                Description = planForCreate.Description,
                Weeks = planForCreate.Weeks,
                HealthIssues = planForCreate.HealthIssues,
                Totalprice = totalCost,
                CreatedDate = DateTime.UtcNow,
                PlanItems = new List<PlanItem>(),
                User = user
            };

            foreach (var selected in planForCreate.SelectedClasses)
            {
                if (!dbClasses.TryGetValue(selected.Id, out var dbClass))
                {
                    ModelState.AddModelError("ClassNotFound", $"Error! Class with Id {selected.Id} not found.");
                    continue;
                }
                plan.PlanItems.Add(new PlanItem(dbClass.Price)
                {
                    Class = dbClass,
                    Goal = planForCreate.Goals?.FirstOrDefault(g => g.ClassId == dbClass.Id)?.Goal
                });

                totalCost += dbClass.Price * planForCreate.Weeks;

            }
            plan.Totalprice = totalCost;
            _context.Plans.Add(plan);


            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ValidationProblem(ModelState));
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} - {ex.Message}");
                return Conflict("There was a problem saving your plan. Please try again later.");
            }

            var response = new PlanResponseDTO
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Weeks = plan.Weeks,
                HealthIssues = plan.HealthIssues,
                Totalprice = plan.Totalprice,
                Classes = plan.PlanItems.Select(pi =>
                {
                    var dbClass = dbClasses[pi.ClassId];
                    return new ClassResponseDTO
                    {
                        ClassId = pi.ClassId,
                        Name = dbClass.Name,
                        Price = dbClass.Price,
                        Date = dbClass.Date,
                        Types = new List<string> { dbClass.ItemType.Name },
                        Goal = pi.Goal
                    };
                }).ToList()
            };

            return CreatedAtAction("GetPlanDetails", new { id = plan.Id }, response);
        }

        //details

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<PlanDetailDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPlanDetails(int id)
        {
            if (_context.Plans == null)
            {
                _logger.LogError(DateTime.Now + " Plans table does not exist.");
                return NotFound();
            }

            PlanDetailDTO? planDetails = await _context.Plans
             .Where(p => p.Id == id)
            .Include(p => p.User)
            .Include(p => p.PlanItems)
            .ThenInclude(pc => pc.Class)
            .ThenInclude(c => c.ItemType)
                   .Select(p => new PlanDetailDTO(
                    p.Id,
                    p.User.UserName,
                    p.CreatedDate,
                    p.Totalprice,
                    p.Name, //?? string.Empty,
                    p.Description, // ?? string.Empty,
                    p.Weeks,
                    p.HealthIssues, // ?? string.Empty,
                    p.PlanItems.Select(pc => new ClassForPlanDTO(
                        pc.Class.Id,
                        pc.Class.Price,
                        pc.Class.Date,
                        pc.Class.Name, // ?? string.Empty,
                        pc.Class.Capacity,
                        new List<string> { pc.Class.ItemType.Name }
                    )).ToList()
                )).FirstOrDefaultAsync();

            if (planDetails == null)
            {
                _logger.LogError(DateTime.Now + $" Plan with id {id} does not exist.");
                return NotFound();
            }

            return Ok(planDetails);

        }
    }
}