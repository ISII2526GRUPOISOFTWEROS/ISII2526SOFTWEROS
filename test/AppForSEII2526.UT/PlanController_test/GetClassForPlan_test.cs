using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
            var typeItems = new List<ItemType>
            {
                new ItemType("Yoga Mat"),
                new ItemType("Dumbbells"),
                new ItemType("Resistance Bands"),
               };
            _context.ItemTypes.AddRange(typeItems);
            _context.SaveChanges();

            var classes = new List<Class>
            {
            new Class(1, 15, "Morning Yoga", 15, DateTime.Today.AddDays(2), new List<PlanItem>(), new List<ItemType>()),
            new Class(2, 10, "HIIT Session", 20, DateTime.Today.AddDays(5), new List<PlanItem>(), new List<ItemType>()),
            };
            classes[0].TypeItems.Add(typeItems[0]); // Yoga Mat
            classes[1].TypeItems.Add(typeItems[1]); // Dumbbells

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

            var result = await controller.GetClassForPlan(new List<string> { "Dumbbells" }, null, null, null);

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

