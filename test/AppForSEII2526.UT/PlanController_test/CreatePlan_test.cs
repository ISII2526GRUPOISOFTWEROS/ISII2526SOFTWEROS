using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII2526.UT.Plan_test
{
    public class CreatePlan_test : AppForSEII25264SqliteUT
    {
        public CreatePlan_test()
        {
            var pm = new TestPaymentMethod() { Name = "Credit Card" };
           
            _context.SaveChanges();

            var type = new ItemType() { Name = "Cardio" };
            _context.ItemTypes.Add(type);
            _context.SaveChanges();

            var cls = new Class(
                0, // id 
                10, // capacity
                "Morning Yoga", // name
                15, // price
                DateTime.Today.AddDays(2), // date
                new List<PlanItem>(), // planItems
                new List<ItemType> { type } // typeItems
            );
            _context.Classes.Add(cls);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("CreatePlan", "Unit Testing")]
        public async Task CreatePlan_Success()
        {
            var mockLogger = new Mock<ILogger<PlanController>>();
            var controller = new PlanController(_context, mockLogger.Object);

            // Reemplaza la inicialización de PlanForCreateDTO usando el constructor requerido
            var dto = new PlanForCreateDTO(
                "Basic Plan", // Name
                "For beginners", // Description
                4, // Weeks
                "None", // HealthIssues
                new List<ClassForPlanDTO>
                {
                    new ClassForPlanDTO(1, 15, DateTime.Today.AddDays(2), "Morning Yoga", 10, new List<string>{"Cardio"})
                },
                new TestPaymentMethod { Id = 1, Name = "Credit Card" } // PaymentMethod
            );

            var result = await controller.CreatePlan(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetPlanById", created.ActionName);
        }

        [Fact]
        [Trait("CreatePlan", "Unit Testing")]
        public async Task CreatePlan_NoClasses_ReturnsBadRequest()
        {
            var mockLogger = new Mock<ILogger<PlanController>>();
            var controller = new PlanController(_context, mockLogger.Object);

            var dto = new PlanForCreateDTO(
                "Empty Plan", // Name
                "No classes", // Description
                2, // Weeks
                null, // HealthIssues
                new List<ClassForPlanDTO>(),
                new TestPaymentMethod { Id = 1, Name = "Credit Card" } // PaymentMethod
            );

            var result = await controller.CreatePlan(dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("You must select at least one class", bad.Value.ToString());
        }

        [Fact]
        [Trait("CreatePlan", "Unit Testing")]
        public async Task CreatePlan_InvalidPaymentMethod_ReturnsBadRequest()
        {
            var mockLogger = new Mock<ILogger<PlanController>>();
            var controller = new PlanController(_context, mockLogger.Object);

            var dto = new PlanForCreateDTO(
                "Invalid PM Plan", // Name
                "Bad payment method", // Description
                2, // Weeks
                null, // HealthIssues
                new List<ClassForPlanDTO>
                {
                    new ClassForPlanDTO(1, 15, DateTime.Today.AddDays(2), "Morning Yoga", 10, new List<string>{"Cardio"})
                },
                new TestPaymentMethod { Id = 999, Name = "Fake" } // PaymentMethod
            );

            var result = await controller.CreatePlan(dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("The selected payment method is not valid", bad.Value.ToString());
        }
    }

    public class TestPaymentMethod : PaymentMethod
    {
        public string Name { get; set; }
    }
}
