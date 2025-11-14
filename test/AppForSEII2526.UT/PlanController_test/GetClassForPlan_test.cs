using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.Classes_test
{
    public class GetClassForPlan_test : AppForSEII25264SqliteUT
    {
        public GetClassForPlan_test()
        {
            var itemTypes = new List<ItemType>()
            {
                new ItemType(){ Name="Strength Equipment"},
                new ItemType(){ Name="Cardio Equipment"},
            };

            _context.ItemTypes.AddRange(itemTypes);
            _context.SaveChanges();

            var classes = new List<Class>
            {
            new Class(1, 15, "Morning Yoga", 15, DateTime.Today.AddDays(2), new List<PlanItem>(), new List<ItemType>()),
            new Class(2, 10, "HIIT Session", 20, DateTime.Today.AddDays(5), new List<PlanItem>(), new List<ItemType>()),
            };
            classes[0].TypeItems.Add(itemTypes[0]); 
            classes[1].TypeItems.Add(itemTypes[1]); 
            

            _context.Classes.AddRange(classes);



            var user = new ApplicationUser()
            {
                Id = "3",
                UserName = "test",
                Surname = "user",
                Email = "test@test.com",
            };
            _context.Users.Add(user);

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
