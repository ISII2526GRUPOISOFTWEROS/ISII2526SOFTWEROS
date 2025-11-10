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
            var user = new ApplicationUser()
            {
                Id = "2",
                UserName = "test",
                Surname = "user",
                Email = "test@test.com",
            };
            _context.Users.AddRange(user);
            _context.SaveChanges();
            var paymentMethods = new List<CreditCard>()
            {
                new CreditCard()
                {
                    Id = 1,
                    User = user,
                    CreditCardNumber = "664543223",
                    ExpirationDate = new DateTime(2027, 12, 31)
                },

            };
            _context.CreditCards.AddRange(paymentMethods);
            _context.SaveChanges();


            var cls = new Class(
                0, // id 
                10, // capacity
                "Morning Yoga", // name
                15, // price
                DateTime.Today.AddDays(2), // date
                new List<PlanItem>(), // planItems
                new List<ItemType> () // typeItems
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

            var dto = new PlanForCreateDTO(
                "Basic Plan", 
                "For beginners", 
                4, 
                "None", 
                new List<ClassForPlanDTO>
                {
                    new ClassForPlanDTO(1, 15, DateTime.Today.AddDays(2), "Morning Yoga", 10, new List<string>{"Cardio"})
                },
                new TestPaymentMethod { Id = 3, Name = "Credit Card" } 
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
                "Empty Plan",
                "No classes",
                2, 
                null, 
                new List<ClassForPlanDTO>(),
                new TestPaymentMethod { Id = 1, Name = "Credit Card" } 
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
