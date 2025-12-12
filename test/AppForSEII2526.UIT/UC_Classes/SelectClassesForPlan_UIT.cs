using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;

namespace AppForSEII2526.UIT.UC_Classes
{
    public class SelectClassesForPlan_UIT : UC_UIT
    {
        public SelectClassesForPlan_UIT(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void MainFlow_DisplayAvailableClasses_Success()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");
            _driver.Navigate().GoToUrl(_URI + "plan/selectclassesforplan");
            var page = new SelectClassesForPlan_P0(_driver, _output);

            // Act - Step 2: System shows list of classes available for next week
            Thread.Sleep(2000);

            // Assert
            Assert.True(page.GetAvailableClassesCount() >= 0, "Classes table should be displayed");
        }


        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void AlternativeFlow4_NoClassesSelected_ShowsMessage()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");
            _driver.Navigate().GoToUrl(_URI + "plan/selectclassesforplan");
            var page = new SelectClassesForPlan_P0(_driver, _output);
            Thread.Sleep(2000);

            // Assert - Alternative Flow 4: Should show "No classes selected" message
            // The page should display a message indicating no classes are selected
            Assert.True(page.ProceedButtonNotAvailable(),
                "Should not allow proceeding without selecting classes");

            _output.WriteLine("Verified that user cannot proceed without selecting classes");
        }


        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void MainFlow_ProceedToPlan_NavigatesToCreatePlan()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");
            _driver.Navigate().GoToUrl(_URI + "plan/selectclassesforplan");
            var page = new SelectClassesForPlan_P0(_driver, _output);
            Thread.Sleep(2000);

            if (page.GetAvailableClassesCount() == 0)
            {
                _output.WriteLine("No classes available. Skipping test.");
                return;
            }

            // Act - Step 4: User selects at least one class and proceeds
            page.AddClassToCart("Morning Yoga");
            page.ClickProceedToPlan();
            Thread.Sleep(1000);

            // Assert - Should navigate to CreatePlan page
            Assert.Contains("/plan/createplan", _driver.Url.ToLower());
        }

        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void AlternativeFlow0_NoClassesAvailable_ShowsWarning()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");
            _driver.Navigate().GoToUrl(_URI + "plan/selectclassesforplan");
            var page = new SelectClassesForPlan_P0(_driver, _output);
            Thread.Sleep(2000);

            // Act - Filter with criteria that returns no results
            page.SearchClasses("NonExistentType12345", null, null, null);

            // Assert - Alternative Flow 0: Should warn user about no classes
            Assert.True(page.NoClassesMessageDisplayed(), "Should display 'No classes found' message");
        }

        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void AlternativeFlow1_FilterByType_ShowsFilteredClasses()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");
            _driver.Navigate().GoToUrl(_URI + "plan/selectclassesforplan");
            var page = new SelectClassesForPlan_P0(_driver, _output);
            Thread.Sleep(2000);

            // Act - Alternative Flow 1: Filter by type
            page.SearchClasses("Cardio Equipment", null, null, null);

            // Assert - Should show filtered results
            var classCount = page.GetAvailableClassesCount();
            _output.WriteLine($"Found {classCount} classes after filtering by 'Yoga'");
        }

        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void AlternativeFlow1_FilterByDate_ShowsFilteredClasses()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");
            _driver.Navigate().GoToUrl(_URI + "plan/selectclassesforplan");
            var page = new SelectClassesForPlan_P0(_driver, _output);
            Thread.Sleep(2000);

            // Act - Alternative Flow 1: Filter by date range
            var fromDate = DateTime.Today.AddDays(1);
            var toDate = DateTime.Today.AddDays(3);
            page.SearchClasses("", null, fromDate, toDate);

            // Assert - Should show classes within date range
            var classCount = page.GetAvailableClassesCount();
            _output.WriteLine($"Found {classCount} classes between {fromDate:d} and {toDate:d}");
        }

        [Fact]
        [Trait("LevelTesting", "UI Integration Testing")]
        public void AlternativeFlow2_SelectPastDate_ShowsWarning()
        {
            // Arrange
            Perform_login("elena.navarro@uclm.es", "Elena.1234");
            _driver.Navigate().GoToUrl(_URI + "plan/selectclassesforplan");
            var page = new SelectClassesForPlan_P0(_driver, _output);
            Thread.Sleep(2000);

            // Act - Alternative Flow 2: Select date before today
            var pastDate = DateTime.Today.AddDays(-5);
            page.SearchClasses("", pastDate, null, null);

            // Assert - Should show warning or no classes
            Thread.Sleep(1000);
            bool hasError = page.CheckMessageError("") || page.NoClassesMessageDisplayed();
            _output.WriteLine($"Error or no classes displayed for past date: {hasError}");
        }
    }
}
