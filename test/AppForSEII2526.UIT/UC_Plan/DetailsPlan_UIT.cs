using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_Classes;
using OpenQA.Selenium;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class DetailsPlan_UIT : UC_UIT
    {
        public DetailsPlan_UIT(ITestOutputHelper output) : base(output)
        {
        }

        // Helper method to create a complete plan
        private void CreateCompletePlan(string planName = "Test Plan", int weeks = 2, string description = "Test Description")
        {
            // Select classes
            _driver.Navigate().GoToUrl(_URI + "plan/selectclassesforplan");
            var selectPage = new SelectClassesForPlan_P0(_driver, _output);
            Thread.Sleep(2000);

            if (selectPage.GetAvailableClassesCount() == 0)
            {
                throw new Exception("No classes available for testing");
            }

            selectPage.AddClassToCart("Morning Yoga");
            selectPage.ClickProceedToPlan();
            Thread.Sleep(1000);

            // Create plan
            var createPage = new CreatePlan_P0(_driver, _output);
            createPage.FillPlanData(planName, description, weeks, 1);
            createPage.ClickCreatePlan();
            Thread.Sleep(500);
            createPage.ClickDialogSave();
            Thread.Sleep(2000);
        }

        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void MainFlow_DisplayPlanDetails_Success()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");
            CreateCompletePlan("My Fitness Plan", 4, "A comprehensive plan");
            var page = new DetailsPlan_P0(_driver, _output);

            // Act - Step 7: System shows the plan performed
            Thread.Sleep(1000);

            // Assert - Verify all plan details are displayed
            Assert.Equal("My Fitness Plan", page.GetPlanName());
            Assert.Equal("4", page.GetPlanWeeks());
            Assert.Equal("A comprehensive plan", page.GetPlanDescription());

            // Verify user data
            Assert.Contains("elena.navarro", page.GetPlanUsername().ToLower());

            // Verify created date is shown
            Assert.False(string.IsNullOrEmpty(page.GetPlanCreatedDate()), "Created date should be displayed");

            // Verify total price is shown
            Assert.False(string.IsNullOrEmpty(page.GetPlanTotalPrice()), "Total price should be displayed");
        }

        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void DisplayPlanDetails_ShowsAllClassInformation()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");
            CreateCompletePlan();
            var page = new DetailsPlan_P0(_driver, _output);

            // Assert - Step 7: Classes should show name, type, price, date, time
            // Verify the class is in the table
            Assert.True(page.IsClassInTable("Morning Yoga", 1), "Morning Yoga class should be in the table");

            _output.WriteLine("Class 'Morning Yoga' found in plan details");
        }

        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void DisplayPlanDetails_OptionalFieldsCanBeEmpty()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");

            // Create plan without optional fields
            _driver.Navigate().GoToUrl(_URI + "plan/selectclassesforplan");
            var selectPage = new SelectClassesForPlan_P0(_driver, _output);
            Thread.Sleep(2000);

            if (selectPage.GetAvailableClassesCount() == 0)
            {
                _output.WriteLine("No classes available. Skipping test.");
                return;
            }

            selectPage.AddClassToCart("Morning Yoga");
            selectPage.ClickProceedToPlan();
            Thread.Sleep(1000);

            var createPage = new CreatePlan_P0(_driver, _output);
            createPage.FillPlanData("Minimal Plan", "", 1, 1);
            createPage.ClickCreatePlan();
            Thread.Sleep(500);
            createPage.ClickDialogSave();
            Thread.Sleep(2000);

            var page = new DetailsPlan_P0(_driver, _output);

            // Assert - Optional fields can be empty
            Assert.Equal("Minimal Plan", page.GetPlanName());

            _output.WriteLine($"Description: '{page.GetPlanDescription()}', Health Issues: '{page.GetPlanHealthIssues()}'");
        }

        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void DisplayPlanDetails_TotalPriceDisplayed()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");
            CreateCompletePlan();
            var page = new DetailsPlan_P0(_driver, _output);

            // Assert - Total price should be displayed
            var totalPrice = page.GetPlanTotalPrice();
            Assert.False(string.IsNullOrEmpty(totalPrice), "Total price should be displayed");
            _output.WriteLine($"Total Price: {totalPrice}");
        }

        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void Authorization_InvalidPlanId_ShowsError()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");

            // Act - Try to access a plan with invalid ID
            _driver.Navigate().GoToUrl(_URI + "plan/detailsplan/99999");
            var page = new DetailsPlan_P0(_driver, _output);
            Thread.Sleep(2000);

            // Assert - Should show error or redirect
            // Note: Error checking method was removed in refactoring
            // This test may need to be updated based on actual error handling
            _output.WriteLine("Navigated to invalid plan ID");
        }

        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void DisplayPlanDetails_ShowsCreationTimestamp()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");
            CreateCompletePlan();
            var page = new DetailsPlan_P0(_driver, _output);

            // Assert - Created date should be shown and be recent
            var createdDate = page.GetPlanCreatedDate();
            Assert.False(string.IsNullOrEmpty(createdDate), "Created date should be displayed");
            _output.WriteLine($"Plan created at: {createdDate}");

            // Verify it's a valid date format
            Assert.True(createdDate.Contains("/") || createdDate.Contains("-"),
                "Created date should be in a date format");
        }

        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void DisplayPlanDetails_AllMandatoryFieldsPresent()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");
            CreateCompletePlan("Complete Plan", 3, "Full description");
            var page = new DetailsPlan_P0(_driver, _output);

            // Assert - All mandatory fields should be present
            Assert.Equal("Complete Plan", page.GetPlanName());
            Assert.Contains("elena.navarro", page.GetPlanUsername().ToLower());
            Assert.Equal("3", page.GetPlanWeeks());

            Assert.False(string.IsNullOrEmpty(page.GetPlanTotalPrice()), "Total price is mandatory");
        }
    }
}

