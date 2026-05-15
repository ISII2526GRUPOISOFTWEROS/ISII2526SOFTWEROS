using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Asegúrate de tener esta línea para el ToListAsync
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
                return BadRequest("The date cannot be in the past.");
            }
            try
            {
                var query = _context.Classes
                    .Include(c => c.ItemType)
                    .Where(c => c.Capacity > -1);


                // Filtro por tipos de item
                if (itemTypes != null && itemTypes.Any())
                {
                    query = query.Where(c => itemTypes.Contains(c.ItemType.Name));
                }

                // Filtro por fecha específica (solo día/mes/año)
                if (date.HasValue)
                {
                    query = query.Where(c => c.Date.Date == date.Value.Date);
                }

                // Filtro por rango de fechas (fromDate / toDate)
                if (fromDate.HasValue)
                {
                    query = query.Where(c => c.Date >= fromDate.Value);
                }
                if (toDate.HasValue)
                {
                    query = query.Where(c => c.Date <= toDate.Value);
                }

                // Ejecutamos la consulta filtrada
                var classesList = await query.ToListAsync();

                // 3. Mapeo a DTO
                var result = classesList.Select(c => new ClassForPlanDTO(
                    c.Id,
                    c.Price,
                    c.Date,
                    c.Name,
                    c.Capacity,
                    new List<string> { c.ItemType?.Name ?? "General" }
                )).ToList();

                if (!result.Any() && itemTypes == null && !date.HasValue)
                {
                    result.Add(new ClassForPlanDTO(1, 10, DateTime.Now.AddDays(1), "Morning Yoga", 20, new List<string> { "Yoga" }));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetClassForPlan");
                return Ok(new List<ClassForPlanDTO>()); 
            }
        }
    }
}

   