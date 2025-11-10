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
                Id = "3",
                UserName = "test",
                Surname = "user",
                Email = "test@test.com",
            };
            _context.Users.AddRange(user);
            _context.SaveChanges();
            var paymentMethods = new List<CreditCard>()
            {
                new CreditCard(){ Id= 3,User= user, CreditCardNumber= "664543223", ExpirationDate= new DateTime(10,12,2026)} };
            _context.CreditCards.AddRange(paymentMethods);

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
            var user = new ApplicationUser()
            {
                Id = "5",
                UserName = "test5",
                Surname = "user5",
                Email = "test5@test.com",
            };
            var dto = new PlanForCreateDTO(
                "Basic Plan", // Name
                "For beginners", // Description
                4, // Weeks
                "None", // HealthIssues
                new List<ClassForPlanDTO>
                {
                    new ClassForPlanDTO(1, 15, DateTime.Today.AddDays(2), "Morning Yoga", 10, new List<string>{"Cardio"})
                },
                new CreditCard { Id = 5, User = user, CreditCardNumber = "682945623", ExpirationDate = new DateTime(21, 12, 2025) }
            
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
            var user = new ApplicationUser()
            {
                Id = "2",
                UserName = "test2",
                Surname = "user2",
                Email = "test2@test.com",
            };

            var dto = new PlanForCreateDTO(
                "Empty Plan", // Name
                "No classes", // Description
                2, // Weeks
                null, // HealthIssues
                new List<ClassForPlanDTO>(),
                new CreditCard { Id = 2, User = user, CreditCardNumber = "664575623", ExpirationDate = new DateTime(20, 12, 2025) } 
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
            var user = new ApplicationUser()
            {
                Id = "4",
                UserName = "test4",
                Surname = "user4",
                Email = "test4@test.com",
            };

            var dto = new PlanForCreateDTO(
                "Invalid PM Plan", // Name
                "Bad payment method", // Description
                2, // Weeks
                null, // HealthIssues
                new List<ClassForPlanDTO>
                {
                    new ClassForPlanDTO(1, 15, DateTime.Today.AddDays(2), "Morning Yoga", 10, new List<string>{"Cardio"})
                },
                new CreditCard { Id = 4, User = user, CreditCardNumber = "669855623", ExpirationDate = new DateTime(20, 11, 2026) } // PaymentMethod
            );

            var result = await controller.CreatePlan(dto);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("The selected payment method is not valid", bad.Value.ToString());
        }
    }
}