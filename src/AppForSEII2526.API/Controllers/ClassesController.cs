using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;



namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ILogger<ClassesController> _logger;

        public ClassesController(ApplicationDbContext context, ILogger<ClassesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //[HttpGet]
        //[Route("[action]")]
        //[ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]

        //public async Task<ActionResult> ComputeDivision (decimal op1, decimal op2)
        //{
        //    if (op2 == 0){
        //        string error = "Op2 cannot be 0 to compute the division";
        //        _logger.LogError(DateTime.Now + "Error:" + error);
        //        return BadRequest(error);
        //    }
        //    decimal result = op1 / op2;
        //    return Ok(result);
        //}
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ClassForPlanDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetClassForPlan(
          [FromQuery] IList<string>? itemTypes, DateTime? date, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                // fechas
                if (date.HasValue && date.Value.Date < DateTime.Today)
                {
                    string error = "Cannot be before today";
                    _logger.LogWarning(DateTime.Now + " " + error);
                    return BadRequest(error);
                }
                if (fromDate.HasValue && fromDate.Value.Date < DateTime.Today)
                {
                    string error = "Cannot be before today";
                    _logger.LogWarning(DateTime.Now + " " + error);
                    return BadRequest(error);
                }
                if (toDate.HasValue && toDate.Value.Date < DateTime.Today)
                {
                    string error = "Cannot be before today";
                    _logger.LogWarning(DateTime.Now + " " + error);
                    return BadRequest(error);
                }
                // built query
                var query = _context.Classes
                    .Include(c => c.TypeItems)
                    .AsQueryable();
                // Filters
                if (itemTypes != null && itemTypes.Count > 0)
                {
                    query = query.Where(c => c.TypeItems.Any(t => itemTypes.Contains(t.Name)));
                }
                if (date.HasValue)
                {
                    query = query.Where(c => c.Date.Date == date.Value.Date);
                }
                else if (fromDate.HasValue && toDate.HasValue)
                {
                    query = query.Where(c => c.Date.Date >= fromDate.Value.Date && c.Date.Date <= toDate.Value.Date);
                }
                var classes = await query
                  .OrderBy(i => i.Date)
                  .Select(i => new ClassForPlanDTO(i.Id, i.Price, i.Date, i.Name, i.Capacity, i.TypeItems.Select(itemtype => itemtype.Name).ToList()))
                  .ToListAsync();
                if (classes.Count == 0)
                {
                    string error = "No classes available";
                    _logger.LogWarning(DateTime.Now + " " + error);
                    return BadRequest(error);
                }
                return Ok(classes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return BadRequest("Error");
            }
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
                .Select(c => new ClassForPlanDTO(c.Id, c.Price, c.Date, c.Name,c.Capacity, c.TypeItems.Select(t => t.Name).ToList()))
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

            IList<PlanDetailDTO> planDetails = await _context.Plans
                .Where(p => p.Id == id)
                .Include(p => p.User)
                .Include(p => p.PlanItems)
                    .ThenInclude(pc => pc.Class)
                .Select(p => new PlanDetailDTO(
                    p.Id,
                    p.User.Name + " " + p.User.Surname,
                    p.CreatedDate,
                    p.Totalprice,
                    p.Name,
                    p.Description ?? string.Empty,
                    p.Weeks,
                    p.HealthIssues ?? string.Empty,
                    p.PlanItems.Select(pc => new ClassForPlanDTO(
                        pc.Class.Id,
                        pc.Class.Price,
                        pc.Class.Date,
                        pc.Class.Name ?? string.Empty,
                        pc.Class.Capacity,
                        pc.Class.TypeItems.Select(t => t.Name).ToList()
                    )).ToList()
                )).ToListAsync();

            if (planDetails == null || !planDetails.Any())
            {
                _logger.LogError(DateTime.Now + $" Plan with id {id} does not exist.");
                return NotFound();
            }

            // Alternative flow: check class capacity
            foreach (var plan in planDetails)
            {
                var lowCapacityClasses = plan.Classes.Where(c => c.capacity <= 0).ToList();
                if (lowCapacityClasses.Any())
                {
                    return BadRequest(new
                    {
                        Message = "One or more classes do not have enough capacity. Please modify selected classes.",
                        Classes = lowCapacityClasses
                    });
                }
            }

            return Ok(planDetails);

        }
    }
    } 
