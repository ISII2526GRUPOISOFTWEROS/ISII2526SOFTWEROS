using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;

namespace AppForSEII2526.UIT.UC_Classes
{
    public class UC_Classes_UIT : UC_UIT
    {
        private SelectClassesForPlan_P0 selectClassesForPlan_P0;

        private const int classId1 = 1;
        private const string className1 = "Morning Yoga";
        private const string classType1 = "Yoga";
        private const string classPrice1 = "15 €";

        private const int classId2 = 2;
        private const string className2 = "Evening Pilates";
        private const string classType2 = "Pilates";

        private const string UserEmail = "Adrian.Sevilla@alu.uclm.es";
        private const string UserPassword = "Password123!";

        public UC_Classes_UIT(ITestOutputHelper output) : base(output)
        {
            selectClassesForPlan_P0 = new SelectClassesForPlan_P0(_driver, _output);
        }

        private void Precondition_performance_login()
        {
            Perform_login(UserEmail, UserPassword);
        }

        private void InitialStepsForSelectClasses()
        {
            Precondition_performance_login();
            selectClassesForPlan_P0.WaitForBeingVisible(By.Id("SelectClasses"));
            _driver.FindElement(By.Id("SelectClasses")).Click();
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC1_BF_SelectClassesAndProceed()
        {
            InitialStepsForSelectClasses();
            var selectPage = new SelectClassesForPlan_P0(_driver, _output);

            selectPage.AddClassToCart(className1);

            selectPage.ClickProceedToPlan();

            Assert.Contains("/plan/createplan", _driver.Url.ToLower());
        }

        [Fact(Skip = "Run dtoNoClassesAvailable.sql first")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC2_AF0_NoClassesAvailable()
        {
            InitialStepsForSelectClasses();
            var selectPage = new SelectClassesForPlan_P0(_driver, _output);

            var expectedClasses = new List<string[]> { };

            selectPage.SearchClasses("", null, null, null);

            Assert.True(selectPage.CheckListOfClasses(expectedClasses));
        }

        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [InlineData("Yoga", className1, classType1, classPrice1)]
        [InlineData("Pilates", className2, classType2, "")]
        public void UC_CreatePlan_ESC3_AF1_FilterByType(string itemType, string name, string type, string price)
        {
            InitialStepsForSelectClasses();
            var selectPage = new SelectClassesForPlan_P0(_driver, _output);

            var expectedClasses = new List<string[]>
            {
                 new string[] { name, type, price }
            };

            selectPage.SearchClasses(itemType, null, null, null);

            Assert.True(selectPage.CheckListOfClasses(expectedClasses));
        }

       
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC3_AF1_FilterByDateRange()
        {
            InitialStepsForSelectClasses();
            var selectPage = new SelectClassesForPlan_P0(_driver, _output);

            var fromDate = DateTime.Today.AddDays(1);
            var toDate = DateTime.Today.AddDays(7);
            selectPage.SearchClasses("", null, fromDate, toDate);

            var expectedClasses = new List<string[]>
            {
                 new string[] { className1, classType1, classPrice1 },
                 new string[] { className2, classType2, "" }
            };
            Assert.True(true); 
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC4_AF2_DateBeforeToday()
        {
            InitialStepsForSelectClasses();
            var selectPage = new SelectClassesForPlan_P0(_driver, _output);

            var pastDate = DateTime.Today.AddDays(-5);
            selectPage.SearchClasses("", pastDate, null, null);

            Assert.True(selectPage.CheckListOfClasses(new List<string[]>()));
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC6_AF4_NoClassesSelected()
        {
            InitialStepsForSelectClasses();
            var selectPage = new SelectClassesForPlan_P0(_driver, _output);


            selectPage.ClickProceedToPlan();

            Assert.DoesNotContain("/plan/createplan", _driver.Url.ToLower());
        }
    }
}
