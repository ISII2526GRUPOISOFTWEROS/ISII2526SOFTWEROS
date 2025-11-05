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
        [ProducesResponseType(typeof(IList<ClassForPlanDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateClassForPlan(string? className, string? classType)
        {
            IList<ClassForPlanDTO> selectedClasses = await _context.Classes
                .Include(c => c.TypeItems)
                .Where(c => (className == null || c.Name.Contains(className)) && (classType == null || c.TypeItems.Any(t => t.Name == classType)))//por tipo
                .OrderBy(c => c.Date)
                .Select(c => new ClassForPlanDTO(c.Id, c.Price, c.Date, c.Name ?? string.Empty, c.Capacity, c.TypeItems.Select(t => t.Name).ToList()))
                .ToListAsync();
            return Ok(selectedClasses);
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

            //foreach (var plan in planDetails)
            //{
            //    var lowCapacityClasses = plan.Classes.Where(c => c.capacity <= 0).ToList();
            //    if (lowCapacityClasses.Any())
            //    {
            //        return BadRequest(new
            //        {
            //            Message = "One or more classes do not have enough capacity. Please modify selected classes.",
            //            Classes = lowCapacityClasses
            //        });
            //    }
            //}

            return Ok(planDetails);

        }
    }
}