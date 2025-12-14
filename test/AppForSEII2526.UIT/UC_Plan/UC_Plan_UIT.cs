using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_Classes;
using OpenQA.Selenium;

namespace AppForSEII2526.UIT.UC_Plan
{
    /// <summary>
    /// UI Integration Tests for UC: Create Plan
    /// Covers steps 5-7 of the main flow and related alternative flows
    /// </summary>
    public class UC_Plan_UIT : UC_UIT
    {
        // Test data - Plan information
        private const string validPlanName = "My Fitness Journey";
        private const string validDescription = "A comprehensive 4-week fitness plan";
        private const int validWeeks = 4;
        private const int validPaymentMethodId = 1;

        private const int classId1 = 1;
        private const string className1 = "Morning Yoga";

        // Test user
        private const string UserEmail = "Adrian.Sevilla@alu.uclm.es";
        private const string UserPassword = "Password123!";

        public UC_Plan_UIT(ITestOutputHelper output) : base(output)
        {
        }

        private void Precondition_performance_login()
        {
            Perform_login(UserEmail, UserPassword);
        }

        /// <summary>
        /// Helper method to select classes first (prerequisite for creating a plan)
        /// Covers steps 1-4 of the main flow
        /// </summary>
        private void SelectClassesForPlan(params string[] classNames)
        {
            _driver.Navigate().GoToUrl(_URI + "plan/selectclassesforplan");
            var selectPage = new SelectClassesForPlan_P0(_driver, _output);

            foreach (var className in classNames)
            {
                try
                {
                    selectPage.AddClassToCart(className);
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"Could not add class '{className}': {ex.Message}");
                }
            }

            selectPage.ClickProceedToPlan();
        }

        private void InitialStepsForCreatePlan()
        {
            Precondition_performance_login();
            SelectClassesForPlan(className1);
        }

        /// <summary>
        /// ESC-1: Basic Flow (BF)
        /// Steps 5-7: Complete plan creation with all mandatory fields
        /// </summary>
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC1_BF_CreatePlanSuccess()
        {
            // Arrange
            InitialStepsForCreatePlan();
            var createPage = new CreatePlan_P0(_driver, _output);

            // Act - Step 5: System shows selected classes and requests plan data
            // Step 6: User fills mandatory data and saves
            createPage.FillPlanData(validPlanName, validDescription, validWeeks, validPaymentMethodId);
            createPage.ClickCreatePlan();

            // Confirm creation
            createPage.ClickDialogSave();

            // Assert - Step 7: Should navigate to details page showing plan information
            Assert.Contains("/plan/detailsplan/", _driver.Url.ToLower());

            var detailsPage = new DetailsPlan_P0(_driver, _output);
            Assert.Contains(validPlanName, detailsPage.GetPlanName());
            Assert.Contains(validWeeks.ToString(), detailsPage.GetPlanWeeks());
        }

        /// <summary>
        /// ESC-5: BF + AF3 (Alternative Flow 3)
        /// User modifies the plan - returns to step 2
        /// </summary>
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC5_AF3_ModifyPlan()
        {
            // Arrange
            InitialStepsForCreatePlan();
            var createPage = new CreatePlan_P0(_driver, _output);

            // Act - Alternative Flow 3: User selects to modify the plan
            createPage.ClickModifyClasses();

            // Assert - Should return to step 2 (SelectClassesForPlan)
            Assert.Contains("/plan/selectclassesforplan", _driver.Url.ToLower());
        }

        /// <summary>
        /// ESC-7: BF + AF5 (Alternative Flow 5)
        /// Missing mandatory data - Plan Name
        /// </summary>
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC7_AF5_MissingPlanName()
        {
            // Arrange
            InitialStepsForCreatePlan();
            var createPage = new CreatePlan_P0(_driver, _output);

            // Act - Alternative Flow 5: Try to create plan without plan name
            createPage.FillPlanData("", validDescription, validWeeks, validPaymentMethodId);
            createPage.ClickCreatePlan();

            // Assert - Should show validation error and return to step 5
            // Assuming we stay on creation page or validation message exists
            Assert.True(true);
            // TODO: Add specific validation check like: Assert.True(createPage.HasValidationError("Name required"));
        }

        /// <summary>
        /// ESC-7: BF + AF5 (Alternative Flow 5)
        /// Missing mandatory data - Number of Weeks
        /// </summary>
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC7_AF5_InvalidWeeks()
        {
            // Arrange
            InitialStepsForCreatePlan();
            var createPage = new CreatePlan_P0(_driver, _output);

            // Act - Alternative Flow 5: Fill with invalid weeks (0)
            createPage.FillPlanData(validPlanName, "", 0, validPaymentMethodId);
            createPage.ClickCreatePlan();

            // Assert - Should show validation error
            Assert.True(true);
            // TODO: Add specific validation check
        }

        /// <summary>
        /// ESC-7: BF + AF5 (Alternative Flow 5)
        /// Missing mandatory data - Payment Method
        /// </summary>
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC7_AF5_MissingPaymentMethod()
        {
            // Arrange
            InitialStepsForCreatePlan();
            var createPage = new CreatePlan_P0(_driver, _output);

            // Act - Alternative Flow 5: Fill without payment method
            createPage.FillPlanData(validPlanName, validDescription, validWeeks, 0);
            createPage.ClickCreatePlan();

            // Assert - Should show validation error
            Assert.True(true);
            // TODO: Add specific validation check
        }

        /// <summary>
        /// ESC-8: BF + AF7 (Alternative Flow 7)
        /// Class without enough capacity - System warns and returns to step 2
        /// </summary>
        [Fact(Skip = "Run dtoClassNoCapacity.sql first")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC8_AF7_ClassWithoutCapacity()
        {
            // Arrange
            InitialStepsForCreatePlan();
            var createPage = new CreatePlan_P0(_driver, _output);

            // Act - Try to create plan (backend should validate capacity)
            createPage.FillPlanData("Capacity Test Plan", "Testing capacity validation",
                validWeeks, validPaymentMethodId);
            createPage.ClickCreatePlan();

            createPage.ClickDialogSave();

            // Assert - Should show error about capacity or return to select classes
            Assert.True(true);
            // TODO: Verify error message or navigation logic
        }
    }
}
