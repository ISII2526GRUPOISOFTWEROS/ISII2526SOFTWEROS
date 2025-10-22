using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


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
        [Route("action")]
        [ProducesResponseType(typeof(IList<ClassForPlanDTO>), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult> GetClassesForPlan(IList<string> itemType, DateTime? date, DateTime? fromDate, DateTime? toDate)
        //{

        //    if (fromDate != null && toDate != null && fromDate > toDate)
        //    {
               
        //        ModelState.AddModelError("fromDate&toDate", "fromDate must be earlier than toDate");
        //        _logger.LogError($"{DateTime.Now} Error: fromDate must be earlier than toDate");
        //        return BadRequest(new ValidationProblemDetails(ModelState));
        //    }
        //{
        //    fromDate = fromDate == null ? DateTime.Today.AddDays(1) : fromDate;
        //    toDate = toDate == null ? DateTime.Today.AddDays(2) : toDate;

        //        IList<ClassForPlanDTO> classes = await _context.Classes
        ////.Where(i => (i.TypeItems.Where(idate => idate..DateFrom <= toDate
        //                                   //&& idate..DateTo >= fromDate).Count()))
        ////.OrderBy(i => i.TypeItems.Select(itemtype => itemtype.Name))
        //.Select(i => new ClassForPlanDTO(i.Id, i.Price, i.Date, i.Name, i.TypeItems.Select(itemtype => itemtype.Name)
        //    .ToList()))
        //.ToListAsync();





                return Ok(classes);


        }

    }
}
