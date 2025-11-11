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
        private readonly ApplicationUser _userWithCard;
        private readonly CreditCard _validCreditCard;
        private readonly ItemType _cardioType;
        private readonly Class _class;

        public CreatePlan_test()
        {
            _userWithCard = new ApplicationUser()
            {
                Id = "3",
                UserName = "test",
                Surname = "user",
                Email = "test@test.com",
            };
            _context.Users.Add(_userWithCard);
            _context.SaveChanges();

            _validCreditCard = new CreditCard()
            {
                Id = 0,
                User = _userWithCard,
                CreditCardNumber = "664543223",
                ExpirationDate = DateTime.UtcNow.AddMonths(1)
            };
            _context.CreditCards.Add(_validCreditCard);

            _cardioType = new ItemType() { Name = "Cardio" };
            _context.ItemTypes.Add(_cardioType);

            _class = new Class(
                0,
                10,
                "Morning Yoga",
                15,
                DateTime.Today.AddDays(2),
                new List<PlanItem>(),
                new List<ItemType> { _cardioType }
            );
            _context.Classes.Add(_class);

            _context.SaveChanges();
        }

        private PlanController CreateController()
        {
            var mockLogger = new Mock<ILogger<PlanController>>();
            return new PlanController(_context, mockLogger.Object);
        }

        [Fact]
        [Trait("CreatePlan", "Unit Testing")]
        public async Task CreatePlan_Success()
        {
            var controller = CreateController();
            var classEntity = _context.Classes.First();
            var paymentMethod = _context.CreditCards.First(); // tarjeta válida

            var dto = new PlanForCreateDTO(
                "Basic Plan",
                "For beginners",
                4,
                "None",
                new List<ClassForPlanDTO>
                {
                    new ClassForPlanDTO(
                        classEntity.Id,
                        classEntity.Price,
                        classEntity.Date,
                        classEntity.Name,
                        classEntity.Capacity,
                        new List<string> { _cardioType.Name })
                },
                paymentMethod.Id // PaymentMethod valido
            );
            var result = await controller.CreatePlan(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.True(created.StatusCode == 201 || created.StatusCode is null);
            Assert.NotNull(created.Value);
        }

        [Fact]
        [Trait("CreatePlan", "Unit Testing")]
        public async Task CreatePlan_NoClasses_ReturnsBadRequest()
        {
            var controller = CreateController();
            var paymentMethod = _context.CreditCards.First();
            var dto = new PlanForCreateDTO(
                "Plan Without Classes",
                "No classes selected",
                4,
                "None",
                new List<ClassForPlanDTO>(), //lista vacia
                paymentMethod.Id
            );
            var result = await controller.CreatePlan(dto);
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(bad.Value);
        }

        [Fact]
        [Trait("CreatePlan", "Unit Testing")]
        public async Task CreatePlan_InvalidPaymentMethod_ReturnsBadRequest()
        {
            var controller = CreateController();
            var classEntity = _context.Classes.First();

            var dto = new PlanForCreateDTO(
                "Invalid PM Plan",
                "Bad payment method",
                2,
                null,
                new List<ClassForPlanDTO>
                {
                    new ClassForPlanDTO(
                        classEntity.Id,
                        classEntity.Price,
                        classEntity.Date,
                        classEntity.Name,
                        classEntity.Capacity,
                        new List<string> { _cardioType.Name })
                },
                999
            );

            var result = await controller.CreatePlan(dto);
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(bad.Value);
        }
    }
}