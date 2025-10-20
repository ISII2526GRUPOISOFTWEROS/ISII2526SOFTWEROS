using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using AppForSEII2526.API.DTOs.ClassesDTOs;
=======
>>>>>>> 7bf82e7d75ddc39812ab5fde3873899cd3e30541

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
<<<<<<< HEAD
        [HttpGet]
        [Route("action")]
        [ProducesResponseType(typeof(IList<ClassForPlanDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetClassesForPlan(string? itemType)
        {

            IList<ClassForPlanDTO> classes = await _context.Classes
        //.Where(i => i.ItemType. || (itemType == null))
        //.OrderBy(i => i.Type)
        .Select(i => new ClassForPlanDTO(i.Id, i.Price, i.Date, i.Name, i.TypeItems.Select(itemtype => itemtype.Name)
            .ToList()))
        .ToListAsync();




            return Ok(classes);


        }
=======

>>>>>>> 7bf82e7d75ddc39812ab5fde3873899cd3e30541
    }
}
