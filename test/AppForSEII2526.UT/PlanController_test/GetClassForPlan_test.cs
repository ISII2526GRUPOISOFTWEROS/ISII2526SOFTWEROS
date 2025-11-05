using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UT.PlanController_test
{
    public class GetClassForPlan_test : AppForSEII25264SqliteUT
    {

        public GetClassForPlan_test()
        {
            var typeItems = new List<ItemType>
            {
                new(0, "Yoga Mat", new List<Item>()),
                new(0, "Dumbbells", new List<Item>()),
                new(0, "Resistance Bands", new List<Item>())
            };
            var planItems = new List<PlanItem>
            {
                new() { ClassId = 0, PlanId = 0, Goal = null, Price = 20 },
                new() { ClassId = 0, PlanId = 0, Goal = null, Price = 15 },
                new() { ClassId = 0, PlanId = 0, Goal = null, Price = 25 }
            };


            var classes = new List<Class>()
            {
                new Class(1, 15, "Cardio",30,new DateTime(2026, 10, 20),new List<PlanItem> { planItems[0] },new List<ItemType> { typeItems[0] }),
                new Class(2, 20,"Fitness", 25,new DateTime(1926, 02, 23),new List<PlanItem> { planItems[0] }, new List<ItemType> { typeItems[1] }),
                new Class(3, 10,"Strength back",25, new DateTime(2027, 04, 04),new List<PlanItem> { planItems[0] }, new List<ItemType> { typeItems[2] })
            };

            ApplicationUser user = new ApplicationUser("Pepe","López",new List<PaymentMethod>(),new List<Incident>(),new List<Restock>());

            _context.AddRange(typeItems);
            _context.AddRange(planItems);
            _context.AddRange(classes);
            _context.Add(user);
            _context.SaveChanges();

        }

        public static IEnumerable<object[]> TestCasesFor_GetClassForPlan_OK()
        {
            var today = DateTime.Today;

            var ClassesDTOs = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(1, 20, new DateTime(2011, 10, 20), "Fitness", 20,new List<string> { "Yoga Mat" }),
                new ClassForPlanDTO(2,20, new DateTime(1988, 02, 23),"Strength Training", 25,new List<string>{ "Dumbbells" }),
                new ClassForPlanDTO(3,10,new DateTime(2007, 04, 04),"Stretch & Flex",20, new List<string>{ "Resistance Bands" })
            };

            var tc1 = new List<ClassForPlanDTO>() { ClassesDTOs[0], ClassesDTOs[1], ClassesDTOs[2] };
            var tc2 = new List<ClassForPlanDTO>() { ClassesDTOs[1] };
            var tc3 = new List<ClassForPlanDTO>() { ClassesDTOs[0], ClassesDTOs[1] };

            var allTests = new List<object[]>
            {
                new object[] { null, null, null, null, tc1,  },
                new object[] { new List<string>{ "Dumbbells" }, null, null, null, tc2, },
                new object[] { null, null, today.AddDays(1), today.AddDays(2), tc3, }
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetClassForPlan_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetClassForPlan_OK_test(IList<string>? itemTypes, DateTime? date, DateTime? fromDate, DateTime? toDate, IList<ClassForPlanDTO> expectedClasses)
        {
            var controller = new ClassesController(_context, null);
            var result = await controller.GetClassesForPlan(itemTypes, date, fromDate, toDate);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);
            Assert.Equal(expectedClasses, actual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetClassForPlan_BadRequest_test()
        {
            var mock = new Mock<ILogger<ClassesController>>();
            ILogger<ClassesController> logger = mock.Object;
            var controller = new ClassesController(_context, logger);
            var result = await controller.GetClassesForPlan(null, null, DateTime.Today.AddDays(5), DateTime.Today.AddDays(1));
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var problem = problemDetails.Errors.First().Value[0];

            Assert.Equal("fromDate must be earlier than toDate", problem);
        }

    }
}

