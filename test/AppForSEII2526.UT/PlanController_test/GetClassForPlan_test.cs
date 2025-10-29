using System;
using System.Collections.Generic;
using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.Controllers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PlanController_test
{
    public class GetClassForPlan_test : AppForSEII25264SqliteUT
    {

        public GetClassForPlan_test() {
            var itemtypes = new List<itemType> { new itemTypes { "Cardio" }, new itemType { "Strength" } };

            var date = new DateTime { new DateTime((2026, 07, 01),10.0m), new DateTime(2027, 07, 01) };
            var fromDate = new DateTime { new DateTime(2026, 06, 30), new DateTime(2027, 06, 30) };
            var toDate = new DateTime { new DateTime(2026, 07, Day), new DateTime(2027, 07, 30) };

            _context.AddRange(itemtypes);
            _context.AddRange(date);
            _context.AddRange(fromDate);
            _context.AddRange(toDate);
            _context.SaveChanges();

        }
        
}
