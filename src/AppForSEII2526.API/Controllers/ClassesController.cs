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
                var query = _context.Classes
                    .Include(c => c.TypeItems)
                    .Where(c => c.Capacity > 0)
                    .AsQueryable();

               
                if (itemTypes != null && itemTypes.Any())
                {
                    var normalized = itemTypes.Select(t => t.ToLower()).ToList();
                    query = query.Where(c => c.TypeItems.Any(t => t.Name != null && normalized.Contains(t.Name.ToLower())));
                }

                if (date.HasValue)
                {
                    var start = date.Value;
                    var end = start.AddSeconds(1); 
                    query = query.Where(c => c.Date >= start && c.Date < end);
                }

                if (fromDate.HasValue && toDate.HasValue)
                {
                    var start = fromDate.Value.Date;
                    var end = toDate.Value.Date.AddDays(1); 
                    query = query.Where(c => c.Date >= start && c.Date < end);
                }

                if ((itemTypes == null || !itemTypes.Any()) && !date.HasValue && !fromDate.HasValue && !toDate.HasValue)
                {
                    var start = DateTime.Today;
                    var end = start.AddDays(7);
                    query = query.Where(c => c.Date >= start && c.Date < end);
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

                if (!classes.Any())
                    return BadRequest("There are no classes available.");

                return Ok(classes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting classes for plan");
                return BadRequest("There are no classes available.");
            }
        }
    }
}
