//using AppForSEII2526.UT;
//using AppForSEII2526.API.Controllers;
//using AppForSEII2526.API.DTOs.ClassesDTOs;
//using AppForSEII2526.API.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Microsoft.EntityFrameworkCore;

//namespace AppForSEII2526.UT.ClassesController_test
//{
//    public class GetClassForPlan_test : AppForSEII25264SqliteUT
//    {
//        public GetClassForPlan_test()
//        {
//            var types = new List<ItemType>()
//            {
//                new ItemType { Name = "Yoga" },
//                new ItemType { Name = "Pilates" },
//            };

//            var classes = new List<Class>()
//            {
//                new Class
//                {
//                    Name = "Morning Yoga",
//                    Date = DateTime.Today.AddDays(1),
//                    Price = 10.0m,
//                    Capacity = 5,
//                    TypeItems = new List<ItemType>{ types[0] }
//                },
//                new Class
//                {
//                    Name = "Evening Pilates",
//                    Date = DateTime.Today.AddDays(2),
//                    Price = 15.0m,
//                    Capacity = 10,
//                    TypeItems = new List<ItemType>{ types[1] }
//                },
//                new Class
//                {
//                    Name = "Old Class",
//                    Date = DateTime.Today.AddDays(-5),
//                    Price = 20.0m,
//                    Capacity = 0,
//                    TypeItems = new List<ItemType>{ types[1] }
//                }
//            };

//            _context.AddRange(types);
//            _context.AddRange(classes);
//            _context.SaveChanges();
//        }

//        public static IEnumerable<object[]> TestCasesFor_GetClassForPlan_OK()
//        {
//            var classDTOs = new List<ClassForPlanDTO>()
//            {
//                new ClassForPlanDTO(1, 10.0m, DateTime.Today.AddDays(1), "Morning Yoga", 5, new List<string>{"Yoga"}),
//                new ClassForPlanDTO(2, 15.0m, DateTime.Today.AddDays(2), "Evening Pilates", 10, new List<string>{"Pilates"})
//            };

//            var tcAll = new List<ClassForPlanDTO> { classDTOs[0], classDTOs[1] };
//            var tcYoga = new List<ClassForPlanDTO> { classDTOs[0] };
//            var tcPilates = new List<ClassForPlanDTO> { classDTOs[1] };

//            return new List<object[]>
//            {
//                new object[] { null, null, null, null, tcAll }, // Sin filtros: devuelve todo por defecto
//                new object[] { new List<string>{"Yoga"}, null, null, null, tcYoga }, // Filtra por tipo
//                new object[] { new List<string>{"Pilates"}, null, null, null, tcPilates }, // Otro tipo
//            };
//        }

//        [Theory]
//        [Trait("LevelTesting", "Unit Testing")]
//        [MemberData(nameof(TestCasesFor_GetClassForPlan_OK))]
//        public async Task GetClassForPlan_filter_test(
//            IList<string>? itemTypes,
//            DateTime? date,
//            DateTime? fromDate,
//            DateTime? toDate,
//            List<ClassForPlanDTO> expectedClasses)
//        {
//            var logger = new LoggerFactory().CreateLogger<ClassesController>();
//            var controller = new ClassesController(_context, logger);

//            var result = await controller.GetClassForPlan(itemTypes, date, fromDate, toDate);
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var actualClasses = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);
//            Assert.Equal(expectedClasses, actualClasses);
//        }
//    }
//}
