using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII2526.UT.Classes_test
{
    public class GetClassForPlan_test : AppForSEII25264SqliteUT
    {
        public GetClassForPlan_test()
        {
            var typeItems = new List<ItemType>()
            {
                new ItemType() { Name = "Cardio" },
                new ItemType() { Name = "Strength" }
            };

            var planItems = new List<PlanItem>
            {
                new PlanItem { ClassId = 1, PlanId = 1, Goal = "Goal1", Price = 10 },
                new PlanItem { ClassId = 2, PlanId = 2, Goal = "Goal2", Price = 20 },
                new PlanItem { ClassId = 3, PlanId = 3, Goal = "Goal3", Price = 30 }
            };

            _context.ItemTypes.AddRange(typeItems);
            _context.SaveChanges();

            var classes = new List<Class>()
            {
                new(1, 15, "Morning Yoga", 15, DateTime.Today.AddDays(2), new List<PlanItem>{ planItems[0] }, new List<ItemType>{ typeItems[0] }),
                new(2, 10, "HIIT Session", 20, DateTime.Today.AddDays(5), new List<PlanItem>{ planItems[1] }, new List<ItemType>{ typeItems[1] }),
                new(3, 12, "Evening Cardio", 25, DateTime.Today.AddDays(7), new List<PlanItem>{ planItems[2] }, new List<ItemType>{ typeItems[0] }),
            };
            _context.Classes.AddRange(classes);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("GetClassForPlan", "Unit Testing")]
        public async Task GetClassForPlan_NoFilters_ReturnsAll()
        {
            var mockLogger = new Mock<ILogger<ClassesController>>();
            var controller = new ClassesController(_context, mockLogger.Object);

            var result = await controller.GetClassForPlan(null, null, null, null);

            var ok = Assert.IsType<OkObjectResult>(result);
            var values = Assert.IsType<List<ClassForPlanDTO>>(ok.Value);

            Assert.Equal(3, values.Count);
        }

        [Fact]
        [Trait("GetClassForPlan", "Unit Testing")]
        public async Task GetClassForPlan_FilterByType_ReturnsFiltered()
        {
            var mockLogger = new Mock<ILogger<ClassesController>>();
            var controller = new ClassesController(_context, mockLogger.Object);

            var result = await controller.GetClassForPlan(new List<string> { "Cardio" }, null, null, null);

            var ok = Assert.IsType<OkObjectResult>(result);
            var values = Assert.IsType<List<ClassForPlanDTO>>(ok.Value);

            Assert.Equal(2, values.Count);
        }

        [Fact]
        [Trait("GetClassForPlan", "Unit Testing")]
        public async Task GetClassForPlan_InvalidDate_ReturnsBadRequest()
        {
            var mockLogger = new Mock<ILogger<ClassesController>>();
            var controller = new ClassesController(_context, mockLogger.Object);

            var result = await controller.GetClassForPlan(null, DateTime.Today.AddDays(-1), null, null);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cannot be before today", bad.Value);
        }
    }
}
