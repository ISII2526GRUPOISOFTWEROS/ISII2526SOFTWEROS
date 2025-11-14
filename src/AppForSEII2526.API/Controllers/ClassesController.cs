using AppForSEII2526.API.DTOs.ClassesDTOs;
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
      [FromQuery] IList<string>? itemTypes,
      DateTime? date,
      DateTime? fromDate,
      DateTime? toDate)
        {
            try
            {
                if ((date.HasValue && date.Value.Date < DateTime.Today) ||
                    (fromDate.HasValue && fromDate.Value.Date < DateTime.Today) ||
                    (toDate.HasValue && toDate.Value.Date < DateTime.Today))
                {
                    string error = "The selected date cannot be before today.";
                    _logger.LogWarning($"{DateTime.Now} {error}");
                    return BadRequest(error);
                }

                var query = _context.Classes
                    .Include(c => c.TypeItems)
                    .AsQueryable();

                if (!itemTypes?.Any() == true && !date.HasValue && !fromDate.HasValue && !toDate.HasValue)
                {
                    DateTime start = DateTime.Today;
                    DateTime end = DateTime.Today.AddDays(7);

                    query = query.Where(c => c.Date.Date >= start && c.Date.Date <= end);
                }

                if (itemTypes != null && itemTypes.Count > 0)
                {
                    var normalized = itemTypes.Select(t => t.ToLower()).ToList();
                    query = query.Where(c => c.TypeItems.Any(t => normalized.Contains(t.Name.ToLower())));
                }

                if (date.HasValue)
                {
                    query = query.Where(c => c.Date.Date == date.Value.Date);
                }

                if (fromDate.HasValue && toDate.HasValue)
                {
                    query = query.Where(c =>
                        c.Date.Date >= fromDate.Value.Date &&
                        c.Date.Date <= toDate.Value.Date);
                }

                var classes = await query
                    .OrderBy(c => c.Date)
                    .Select(c => new ClassForPlanDTO(
                        c.Id,
                        c.Price,
                        c.Date,
                        c.Name,
                        c.Capacity,
                        c.TypeItems.Select(t => t.Name).ToList()))
                    .ToListAsync();

                if (classes.Count == 0)
                {
                    string error = "No classes available.";
                    _logger.LogWarning($"{DateTime.Now} {error}");
                    return BadRequest(error);
                }

                return Ok(classes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting classes for plan");
                return BadRequest("An error occurred while retrieving classes.");
            }
        }
    }
}
