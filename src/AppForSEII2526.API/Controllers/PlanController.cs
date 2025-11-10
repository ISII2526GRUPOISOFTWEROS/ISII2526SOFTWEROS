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
            if (!ModelState.IsValid)
            {
                return BadRequest(ValidationProblem(ModelState));
            }

            var paymentMethod = await _context.Set<PaymentMethod>()
                .FirstOrDefaultAsync(pm => pm.Id == planForCreate.PaymentMethod.Id);
            if (paymentMethod == null){
                ModelState.AddModelError("PaymentMethod", "The selected payment method is invalid or not found.");
                return BadRequest(ValidationProblem(ModelState));
            }
            if (planForCreate.SelectedClasses == null || !planForCreate.SelectedClasses.Any())
            {
                ModelState.AddModelError("SelectedClasses", "You must select at least one class for the plan.");
                return BadRequest(ValidationProblem(ModelState));
            }
            var selectedClassIds = planForCreate.SelectedClasses.Select(c => c.Id).ToList();
            var dbClasses = await _context.Classes
                .Where(c => selectedClassIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id);
            decimal totalCost = 0;
            var planItems = new List<PlanItem>();
            foreach (var selected in planForCreate.SelectedClasses)
            {
                if (!dbClasses.TryGetValue(selected.Id, out var dbClass))
                {
                    ModelState.AddModelError("ClassNotFound", $"Error! Class with Id {selected.Id} not found.");
                    continue;
                }
                // precio por clase
                totalCost += dbClass.Price * planForCreate.Weeks;
                planItems.Add(new PlanItem(dbClass.Price)
                {
                    ClassId = dbClass.Id,
                    Price = dbClass.Price,
                });
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ValidationProblem(ModelState));
            }
            var plan = new Plan
            {
                Name = planForCreate.Name,
                Description = planForCreate.Description,
                Weeks = planForCreate.Weeks,
                HealthIssues = planForCreate.HealthIssues,
                Totalprice = totalCost,
                CreatedDate = DateTime.UtcNow,
                PlanItems = planItems
            };

            _context.Plans.Add(plan);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} - {ex.Message}");
                return Conflict("There was a problem saving your plan. Please try again later.");
            }
            return CreatedAtAction("GetPlanById", new { id = plan.Id }, new{
                plan.Id,
                plan.Name,
                plan.Totalprice,
                Classes = planItems.Select(pi => new{
                    pi.ClassId,
                    pi.Price,
                    pi.Goal
                })});
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
                        .ThenInclude(c => c.TypeItems)
                .Select(p => new PlanDetailDTO(
                    p.Id,
                    "hduewi23@gmail.com", ///p.User.UserName,
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
                        pc.Class.TypeItems.Select(t => t.Name).ToList()
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