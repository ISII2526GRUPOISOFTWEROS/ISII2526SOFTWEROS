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
                UserName = _userName,
                Surname = _surname,
                Email = "pepe.gomez@test.com",
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
    new Class { Id = 1, Name = "Morning Yoga", Price = 10.0m, Date = DateTime.Today.AddDays(1), ItemType = types[0], Capacity = 10 }, // <--- PONER 10
    new Class { Id = 2, Name = "Evening Pilates", Price = 15.0m, Date = DateTime.Today.AddDays(2), ItemType = types[1], Capacity = 10 } // <--- PONER 10
};

            _context.AddRange(types);
            _context.AddRange(classes);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePlan_Error()
        {

            var planUserNotRegistered = new PlanForCreateDTO
            {
                UserName = "NoExisteTest",
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
                UserName = _userName,
                Name = "Plan 2",
                Weeks = 4,
                PaymentMethodId = 1,
                SelectedClasses = new List<ClassSelectionDTO>()

            };

            var planInvalidWeeks = new PlanForCreateDTO
            {
                UserName = _userName,
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
                new object[] { planUserNotRegistered, "Error! Username is not registered" },
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
            var errorActual = badRequestResult.Value.ToString();
            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePlan_Success_test()
        {
            var mock = new Mock<ILogger<PlanController>>();
            ILogger<PlanController> logger = mock.Object;

            var controller = new PlanController(_context, logger);
            var class1 = _context.Classes.First(c => c.Name == "Morning Yoga");
            var class2 = _context.Classes.First(c => c.Name == "Evening Pilates");


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
                Id = class1.Id,
                Name = class1.Name,
                Price = class1.Price,
                Date = class1.Date,
                ItemType = new List<string> { class1.ItemType.Name }
            },
            new ClassSelectionDTO
            {
                Id = class2.Id,
                Name = class2.Name,
                Price = class2.Price,
                Date = class2.Date,
                ItemType = new List<string> { class2.ItemType.Name }
            }
        }
            };

            var result = await controller.CreatePlan(planDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<PlanResponseDTO>(createdResult.Value);

            Assert.Equal(planDTO.Name, response.Name);
            Assert.Equal(planDTO.Weeks, response.Weeks);
            Assert.Equal(50m, response.Totalprice);
            Assert.Equal(planDTO.SelectedClasses.Count, response.Classes.Count);


        }
    }
}

