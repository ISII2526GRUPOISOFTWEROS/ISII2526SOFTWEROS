using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForSEII2526.UT.PlanController_test
{
    public class CreatePlan_test : AppForSEII25264SqliteUT
    {
        private const string _userName = "Pepe.Gomez";
        private const string _name = "Pepe";
        private const string _surname = "Gomez";
        private const string _deliveryAddress = "Avda. España s/n, Albacete 02071";

        public CreatePlan_test()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                UserName = "test",
                Surname = "user",
                Email = "test@test.com",
            };
            _context.Users.AddRange(user);
            var paymentMethods = new List<Bizum>()
            {
                new Bizum(){ Id= 1,User= user, TelephoneNumber= 664543223},

            };
            _context.Bizums.AddRange(paymentMethods);
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
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePlan_Error()
        {
            
            var planUserNotRegistered = new PlanForCreateDTO
            {
                UserName = "victor.lopez@uclm.es",
                Name = "Plan 1",
                Weeks = 4,
                PaymentMethodId = 1,
                SelectedClasses = new List<ClassSelectionDTO>
{
    new ClassSelectionDTO
    {
        Id = 1,
        Name = "Morning Yoga",
        Price = 10m,
        Date = DateTime.Today.AddDays(1),
        ItemType = new List<string?> { "Yoga" }
    },
    new ClassSelectionDTO
    {
        Id = 2,
        Name = "Evening Pilates",
        Price = 15m,
        Date = DateTime.Today.AddDays(2),
        ItemType = new List<string?> { "Pilates" }
    }
}
            };


                var planNoClasses = new PlanForCreateDTO
                {
                    UserName = "test",
                    Name = "Plan 2",
                    Weeks = 4,
                    PaymentMethodId = 1,
                    SelectedClasses = new List<ClassSelectionDTO>
{
    new ClassSelectionDTO
    {
        Id = 1,
        Name = "Morning Yoga",
        Price = 10m,
        Date = DateTime.Today.AddDays(1),
        ItemType = new List<string?> { "Yoga" }
    },
    new ClassSelectionDTO
    {
        Id = 2,
        Name = "Evening Pilates",
        Price = 15m,
        Date = DateTime.Today.AddDays(2),
        ItemType = new List<string?> { "Pilates" }
    }
}
                };

                    var planInvalidWeeks = new PlanForCreateDTO
            {
                UserName = "elena@uclm.es",
                Name = "Plan 3",
                Weeks = 0,
                PaymentMethodId = 1,
                        SelectedClasses = new List<ClassSelectionDTO>
{
    new ClassSelectionDTO
    {
        Id = 1,
        Name = "Morning Yoga",
        Price = 10m,
        Date = DateTime.Today.AddDays(1),
        ItemType = new List<string?> { "Yoga" }
    },
    new ClassSelectionDTO
    {
        Id = 2,
        Name = "Evening Pilates",
        Price = 15m,
        Date = DateTime.Today.AddDays(2),
        ItemType = new List<string?> { "Pilates" }
    } }
};

                        var allTests = new List<object[]>
            {
                new object[] { planUserNotRegistered, "Error! Username is not registred" },
                new object[] { planNoClasses, "At least one class must be selected." },
                new object[] { planInvalidWeeks, "Weeks must be greater than 0." }
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreatePlan_Error))]
        public async Task CreatePlan_Error_test(PlanForCreateDTO planDTO, string errorExpected)
        {
            var mock = new Mock<ILogger<PlanController>>();
            ILogger<PlanController> logger = mock.Object;

            var controller = new PlanController(_context, logger);

            var result = await controller.CreatePlan(planDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var errorActual = problemDetails.Errors.First().Value[0];
            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePlan_Success_test()
        {
            var mock = new Mock<ILogger<PlanController>>();
            ILogger<PlanController> logger = mock.Object;

            var controller = new PlanController(_context, logger);

            var planDTO = new PlanForCreateDTO
            {
                UserName = _userName,
                Name = "Healthy Plan",
                Weeks = 2,
                PaymentMethodId = 1,
                SelectedClasses = new List<ClassSelectionDTO>
{
    new ClassSelectionDTO
    {
        Id = 1,
        Name = "Morning Yoga",
        Price = 10m,
        Date = DateTime.Today.AddDays(1),
        ItemType = new List<string?> { "Yoga" }
    },
    new ClassSelectionDTO
    {
        Id = 2,
        Name = "Evening Pilates",
        Price = 15m,
        Date = DateTime.Today.AddDays(2),
        ItemType = new List<string?> { "Pilates" }
    } }
};

                var result = await controller.CreatePlan(planDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            dynamic response = createdResult.Value;

            Assert.Equal(planDTO.Name, (string)response.Name);
            Assert.Equal(planDTO.Weeks, (int)response.Weeks);
            Assert.Equal(50m, (decimal)response.Totalprice); 
            Assert.Equal(planDTO.SelectedClasses.Count, ((IEnumerable<dynamic>)response.Classes).Count());
        }
    }
}
