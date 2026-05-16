using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.Linq;
using System.Net;

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
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ClassForPlanDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetClassForPlan(
    [FromQuery] IList<string>? itemTypes,
    DateTime? date,
    DateTime? fromDate,
    DateTime? toDate)
        {
            if (date.HasValue && date.Value < DateTime.Today)
            {
                
                return BadRequest("LA FECHA NO ES VALIDA");
            }

            try
            {
                var query = _context.Classes
                    .Include(c => c.ItemType)
                    .Where(c => c.Capacity >= 0) 
                    .AsQueryable();

                // Filtro por tipos de item
                if (itemTypes != null && itemTypes.Any(t => !string.IsNullOrEmpty(t)))
                {
                    query = query.Where(c => itemTypes.Contains(c.ItemType.Name));
                }

                // Filtro por fecha específica
                if (date.HasValue)
                {
                    query = query.Where(c => c.Date.Date == date.Value.Date);
                }

                // Filtro por rango de fechas
                if (fromDate.HasValue)
                {
                    query = query.Where(c => c.Date.Date >= fromDate.Value.Date);
                }
                if (toDate.HasValue)
                {
                    query = query.Where(c => c.Date.Date <= toDate.Value.Date);
                }

                var classesList = await query.ToListAsync();

                var result = classesList.Select(c => new ClassForPlanDTO(
                    c.Id,
                    c.Price,
                    c.Date,
                    c.Name,
                    c.Capacity,
                    new List<string> { c.ItemType?.Name ?? "General" }
                )).ToList();


                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetClassForPlan");
                // En caso de error crítico, devolvemos una lista vacía para no romper el Front
                return Ok(new List<ClassForPlanDTO>());
            }
        }
    }
}

   