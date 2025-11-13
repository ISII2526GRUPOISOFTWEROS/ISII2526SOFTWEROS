//using AppForSEII2526.API.Controllers;
//using AppForSEII2526.API.DTOs.ClassesDTOs;
//using AppForSEII2526.API.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Xunit;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AppForSEII2526.UT.Classes_test
//{
//    public class GetClassForPlan_test : AppForSEII25264SqliteUT
//    {
//        public GetClassForPlan_test()
//        {
//            var classes = new List<Class>
//{
//    new Class()
//    {
//        Id = 1,
//        Capacity = 15,
//        Name = "Morning Yoga",
//        Price = 15,
//        Date = DateTime.Today.AddDays(2),
//        PlanItems = new List<PlanItem>(),
//        TypeItems = new List<ItemType>() 
//    },
//    new Class()
//    {
//        Id = 2,
//        Capacity = 10,
//        Name = "HIIT Session",
//        Price = 20,
//        Date = DateTime.Today.AddDays(5),
//        PlanItems = new List<PlanItem>(),
//        TypeItems = new List<ItemType>() 
//    },
//    new Class()
//    {
//        Id = 3,
//        Capacity = 12,
//        Name = "Strength Training",
//        Price = 25,
//        Date = DateTime.Today.AddDays(3),
//        PlanItems = new List<PlanItem>(),
//        TypeItems = new List<ItemType>() 
//    }
//};

//            _context.Classes.AddRange(classes);
          


//            var user = new ApplicationUser()
//            {
//                Id = "3",
//                UserName = "test",
//                Surname = "user",
//                Email = "test@test.com",
//            };
//            _context.Users.Add(user);

//            _context.SaveChanges();
//        }

//        [Fact]
//        [Trait("GetClassForPlan", "Unit Testing")]
//        public async Task GetClassForPlan_NoFilters_ReturnsAll()
//        {
//            var mockLogger = new Mock<ILogger<ClassesController>>();
//            var controller = new ClassesController(_context, mockLogger.Object);

//            var result = await controller.GetClassForPlan(null, null, null, null);

//            var ok = Assert.IsType<OkObjectResult>(result);
//            var values = Assert.IsType<List<ClassForPlanDTO>>(ok.Value);

//            Assert.Equal(3, values.Count);
//        }

//        [Fact]
//        [Trait("GetClassForPlan", "Unit Testing")]
//        public async Task GetClassForPlan_InvalidDate_ReturnsBadRequest()
//        {
//            var mockLogger = new Mock<ILogger<ClassesController>>();
//            var controller = new ClassesController(_context, mockLogger.Object);

//            var result = await controller.GetClassForPlan(null, DateTime.Today.AddDays(-1), null, null);

//            var bad = Assert.IsType<BadRequestObjectResult>(result);
//            Assert.Equal("Cannot be before today", bad.Value);
//        }
//    }
//}
