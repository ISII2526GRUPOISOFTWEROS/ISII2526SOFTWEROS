using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Numerics;

namespace AppForSEII2526.UT.PlanController_test
{
    public class GetPlanDetails_test : AppForSEII25264SqliteUT
    {

        private const string _userName = "Pepe.Gomez";
        private const string _surname = "Gomez";

        public GetPlanDetails_test()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                UserName =_userName,
                Surname = _surname,
                Email = "test@test.com",
            };
            _context.Users.AddRange(user);

            var types = new List<ItemType>()
            {
                new ItemType { Name = "Yoga" },
                new ItemType { Name = "Pilates" }
            };

            var classes = new List<Class>()
            {
                new Class { Id = 1, Name = "Morning Yoga", Price = 10.0m, Date = DateTime.Today.AddDays(1), TypeItems = new List<ItemType> { types[0] } },
                new Class { Id = 2, Name = "Evening Pilates", Price = 15.0m, Date = DateTime.Today.AddDays(2), TypeItems = new List<ItemType> { types[1] } }
            };

            _context.AddRange(types);
            _context.AddRange(classes);

            var plan = new Plan
            {
                Name = "My Weekly Plan",
                Description = "Test plan",
                Weeks = 2,
                HealthIssues = "None",
                Totalprice = 0m,
                CreatedDate = DateTime.UtcNow,
                User = user,
                PlanItems = new List<PlanItem>
                {
                    new PlanItem(classes[0].Price) { Class = classes[0] },
                    new PlanItem(classes[1].Price) { Class = classes[1]  }
                }
            };

            plan.Totalprice = plan.PlanItems.Sum(pi => pi.Price * plan.Weeks);

            _context.Plans.Add(plan);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetPlanDetails_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<PlanController>>();
            var logger = mock.Object;

            var controller = new PlanController(_context, logger);

            // Act
            var result = await controller.GetPlanDetails(0);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetPlanDetails_Found_test()
        {

            var mock = new Mock<ILogger<PlanController>>();
            ILogger<PlanController> logger = mock.Object;

            var controller = new PlanController(_context, logger);
            var expectedPlan = new PlanDetailDTO(
                id: 1,
                userName:_userName,
                dateCreated: _context.Plans.First().CreatedDate,
                totalPrice: 50m, // Calculado: (10 + 15) * 2 semanas
                planName: "My Weekly Plan",
                description: "Test plan",
                numberOfWeeks: 2,
                healthIssues: "None",
                classes: new List<ClassForPlanDTO>
                {
                    new ClassForPlanDTO(1, 10m, DateTime.Today.AddDays(1), "Morning Yoga", 5, new List<string?> { "Yoga" }),
                    new ClassForPlanDTO(2, 15m, DateTime.Today.AddDays(2), "Evening Pilates", 10, new List<string?> { "Pilates" })
                }
            );


            var result = await controller.GetPlanDetails(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var planActual = Assert.IsType<PlanDetailDTO>(okResult.Value);
            Assert.Equal(expectedPlan.Id, planActual.Id);
            Assert.Equal(expectedPlan.UserName, planActual.UserName);
            Assert.Equal(expectedPlan.PlanName, planActual.PlanName);
            Assert.Equal(expectedPlan.NumberOfWeeks, planActual.NumberOfWeeks);
            Assert.Equal(expectedPlan.TotalPrice, planActual.TotalPrice);
            Assert.Equal(expectedPlan.Description, planActual.Description);
            Assert.Equal(expectedPlan.HealthIssues, planActual.HealthIssues);
            Assert.Equal(expectedPlan.Classes.Count, planActual.Classes.Count);
            for (int i = 0; i < expectedPlan.Classes.Count; i++)
            {
                Assert.Equal(expectedPlan.Classes[i].Id, planActual.Classes[i].Id);
                Assert.Equal(expectedPlan.Classes[i].Name, planActual.Classes[i].Name);
                Assert.Equal(expectedPlan.Classes[i].date, planActual.Classes[i].date);
                Assert.Equal(expectedPlan.Classes[i].price, planActual.Classes[i].price);
                Assert.Equal(expectedPlan.Classes[i].capacity, planActual.Classes[i].capacity);
                Assert.True(expectedPlan.Classes[i].itemType.SequenceEqual(planActual.Classes[i].itemType));
            }

        }
    }
}
