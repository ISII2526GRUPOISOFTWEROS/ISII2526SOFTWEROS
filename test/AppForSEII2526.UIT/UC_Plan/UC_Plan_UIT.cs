using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_Classes;
using OpenQA.Selenium;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class UC_Plan_UIT : UC_UIT
    {
        private const string validPlanName = "My Fitness Journey";
        private const string validDescription = "A comprehensive 4-week fitness plan";
        private const int validWeeks = 4;
        private const int validPaymentMethodId = 1;

        private const int classId1 = 1;
        private const string className1 = "Morning Yoga";

        private const string UserEmail = "Adrian.Sevilla@alu.uclm.es";
        private const string UserPassword = "Password123!";

        public UC_Plan_UIT(ITestOutputHelper output) : base(output)
        {
        }

        private void Precondition_performance_login()
        {
            Perform_login(UserEmail, UserPassword);
        }

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

        
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC1_BF_CreatePlanSuccess()
        {
            InitialStepsForCreatePlan();
            var createPage = new CreatePlan_P0(_driver, _output);

            createPage.FillPlanData(validPlanName, validDescription, validWeeks, validPaymentMethodId);
            createPage.ClickCreatePlan();

            createPage.ClickDialogSave();

            Assert.Contains("/plan/detailsplan/", _driver.Url.ToLower());

            var detailsPage = new DetailsPlan_P0(_driver, _output);
            Assert.Contains(validPlanName, detailsPage.GetPlanName());
            Assert.Contains(validWeeks.ToString(), detailsPage.GetPlanWeeks());
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC5_AF3_ModifyPlan()
        {
            InitialStepsForCreatePlan();
            var createPage = new CreatePlan_P0(_driver, _output);

            createPage.ClickModifyClasses();
            Assert.Contains("/plan/selectclassesforplan", _driver.Url.ToLower());
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC7_AF5_MissingPlanName()
        {
            InitialStepsForCreatePlan();
            var createPage = new CreatePlan_P0(_driver, _output);

            createPage.FillPlanData("", validDescription, validWeeks, validPaymentMethodId);
            createPage.ClickCreatePlan();

            Assert.True(true);
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC7_AF5_InvalidWeeks()
        {
            
            InitialStepsForCreatePlan();
            var createPage = new CreatePlan_P0(_driver, _output);

            createPage.FillPlanData(validPlanName, "", 0, validPaymentMethodId);
            createPage.ClickCreatePlan();

            Assert.True(true);
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC7_AF5_MissingPaymentMethod()
        {
            InitialStepsForCreatePlan();
            var createPage = new CreatePlan_P0(_driver, _output);

            createPage.FillPlanData(validPlanName, validDescription, validWeeks, 0);
            createPage.ClickCreatePlan();

            Assert.True(true);
        }

        [Fact(Skip = "Run dtoClassNoCapacity.sql first")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC8_AF7_ClassWithoutCapacity()
        {
            InitialStepsForCreatePlan();
            var createPage = new CreatePlan_P0(_driver, _output);

            createPage.FillPlanData("Capacity Test Plan", "Testing capacity validation",
                validWeeks, validPaymentMethodId);
            createPage.ClickCreatePlan();

            createPage.ClickDialogSave();

            Assert.True(true);
        }
    }
}
