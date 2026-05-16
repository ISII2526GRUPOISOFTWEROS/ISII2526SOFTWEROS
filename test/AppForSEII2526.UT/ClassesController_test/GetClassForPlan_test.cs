using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII2526.UT.ClassesController_test
{
    public class GetClassForPlan_test : AppForSEII25264SqliteUT
    {
        public GetClassForPlan_test()
        {
            var types = new List<ItemType>()
            {
                new ItemType { Name = "Yoga" },
                new ItemType { Name = "Pilates" },
            };

            var classes = new List<Class>()
            {
                new Class
                {
                    Id = 1,
                    Name = "Morning Yoga",
                    Date = DateTime.Today.AddDays(1),
                    Price = 10.0m,
                    Capacity = 5,
                    ItemType = types[0]
                },
                new Class
                {
                    Id = 2,
                    Name = "Evening Pilates",
                    Date = DateTime.Today.AddDays(2),
                    Price = 15.0m,
                    Capacity = 10,
                    ItemType = types[1]
                }
            };

            _context.AddRange(types);
            _context.AddRange(classes);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetClassForPlan_NULL_test()
        {
            // ARRANGE
            var expectedClasses = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(1, 10.0m, DateTime.Today.AddDays(1), "Morning Yoga", 5, new List<string>{"Yoga"}),
                new ClassForPlanDTO(2, 15.0m, DateTime.Today.AddDays(2), "Evening Pilates", 10, new List<string>{"Pilates"})
            };

            var mock = new Mock<ILogger<ClassesController>>();
            ClassesController controller = new ClassesController(_context, mock.Object);

            // ACT
            var result = await controller.GetClassForPlan(null, null, null, null);

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);

            
            Assert.Equal(expectedClasses.Count, actualResult.Count);
            Assert.Equal(expectedClasses[0].Name, actualResult[0].Name);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetClassForPlan_WrongDate_test()
        {
            // ARRANGE
            var expectedDetail = "LA FECHA NO ES VALIDA";

            var mock = new Mock<ILogger<ClassesController>>();
            ClassesController controller = new ClassesController(_context, mock.Object);

            // ACT date of the past (error)
            var result = await controller.GetClassForPlan(null, DateTime.Today.AddDays(-1), null, null);

            // ASSERT
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result); // Ahora sí será un 400
            var actualMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(expectedDetail, actualMessage);
        }

        // Transform the fact into a theory
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetClassForPlan_OK))]
        public async Task GetClassForPlan_theory(IList<string>? itemTypes, DateTime? date, DateTime? from, DateTime? to, List<ClassForPlanDTO> expectedClasses)
        {
            // ARRANGE
            var mock = new Mock<ILogger<ClassesController>>();
            ClassesController controller = new ClassesController(_context, mock.Object);

            // ACT
            var result = await controller.GetClassForPlan(itemTypes, date, from, to);

            // ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualClasses = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);

            Assert.Equal(expectedClasses.Count, actualClasses.Count);

            for (int i = 0; i < expectedClasses.Count; i++)
            {
                Assert.Equal(expectedClasses[i].Name, actualClasses[i].Name);
                Assert.Equal(expectedClasses[i].price, actualClasses[i].price);
            }
        
        }

        public static IEnumerable<object[]> TestCasesFor_GetClassForPlan_OK()
        {
            // TEST CASES
            var classDTOs = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(1, 10.0m, DateTime.Today.AddDays(1), "Morning Yoga", 5, new List<string>{"Yoga"}),
                new ClassForPlanDTO(2, 15.0m, DateTime.Today.AddDays(2), "Evening Pilates", 10, new List<string>{"Pilates"})
            };

            // Filter by Yoga
            var expectedTC1 = new List<ClassForPlanDTO> { classDTOs[0] };

            // Filter by Date
            var expectedTC2 = new List<ClassForPlanDTO> { classDTOs[1] };

            var allTests = new List<object[]> {
                new object[] { new List<string>{"Yoga"}, null, null, null, expectedTC1 },
                new object[] { null, DateTime.Today.AddDays(2), null, null, expectedTC2 }
            };

            return allTests;
        }
    }
}