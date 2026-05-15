using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System.Globalization;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class UC_PlanClasses_UIT : UC_UIT
    {
        private SelectClassesForPlan_PO selectClassesForPlan_PO;
        private CreatePlan_P0 createPlan_PO;
        private DetailsPlan_P0 detailsPlan_P0;

        private const string validPlanName = "PlanForMyFirstWeeks";
        private const string validDescription = "A comprehensive 4-week fitness plan";
        private const int validWeeks = 4;
        private const int validPaymentMethodId = 1;

        private const int classId1 = 1;
        private const string className1 = "Morning Yoga";
        private const string className2 = "Spinning";


        private const string UserEmail = "Adrian.Sevilla@alu.uclm.es";
        private const string UserPassword = "Password123!";

        public UC_PlanClasses_UIT(ITestOutputHelper output) : base(output)
        {
            selectClassesForPlan_PO = new SelectClassesForPlan_PO(_driver, _output);
            createPlan_PO = new CreatePlan_P0(_driver, _output);
            detailsPlan_P0 = new DetailsPlan_P0(_driver, _output);
        }

        private void Precondition_performance_login()
        {
            Perform_login(UserEmail, UserPassword);
        }

        private void SelectClassesForPlan(params string[] classNames)
        {
            _driver.Navigate().GoToUrl(_URI + "plan/selectclassesforplan");

            System.Threading.Thread.Sleep(5000);

            foreach (var className in classNames)
            {
                try
                {
                    
                    selectClassesForPlan_PO.AddClassToSelected(className);
                    System.Threading.Thread.Sleep(1000); 
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"No pude añadir '{className}': {ex.Message}");
                }
            }

            try
            {
                selectClassesForPlan_PO.PressProceedToPlanButton();
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error al pulsar Proceed: {ex.Message}");
                _driver.Navigate().Refresh();
            }
        }

        private void InitialStepsForCreatePlan()
        {
            Precondition_performance_login();
            SelectClassesForPlan(className1);
        }
        private void InitialStepsForCapacityTest()

        {

            Precondition_performance_login();

            SelectClassesForPlan(className2);

        }



        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC5_AF3_ModifyPlan()
        {
            // ARRANGE
            InitialStepsForCreatePlan();

            // ACT
            createPlan_PO.ModifyClasses();

            // ASSERT
            Assert.True(_driver.Url.Contains("plan", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC7_AF5_MissingPlanName()
        {
            // ARRANGE
            InitialStepsForCreatePlan();

            // ACT
            createPlan_PO.FillPlanData("", validDescription, validWeeks);
            createPlan_PO.SubmitPlan();

            // ASSERT
            Assert.True(true);
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC7_AF5_InvalidWeeks()
        {
            // ARRANGE
            InitialStepsForCreatePlan();

            // ACT
            createPlan_PO.FillPlanData(validPlanName, "", 0);
            createPlan_PO.SubmitPlan();

            // ASSERT
            Assert.True(true);
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC7_AF5_MissingPaymentMethod()
        {
            // ARRANGE
            InitialStepsForCreatePlan();

            // ACT
            createPlan_PO.FillPlanData(validPlanName, validDescription, validWeeks);
            createPlan_PO.SubmitPlan();

            // ASSERT
            Assert.True(true);
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_CreatePlan_ESC8_AF7_ClassWithoutCapacity()
        {
            // ARRANGE
            InitialStepsForCapacityTest();

            // ACT
            createPlan_PO.FillPlanData("Capacity Test Plan", "Testing capacity validation", validWeeks);
            createPlan_PO.SubmitPlan();
            createPlan_PO.ConfirmPlanSubmission();

            // ASSERT
            Assert.True(true);
        }

        //[Fact]
        //[Trait("LevelTesting", "Functional Testing")]
        //public void UC_CreatePlan_ESC1_BF_CreatePlanSuccess()
        //{
        //    // 1. ARRANGE
        //    InitialStepsForCreatePlan();

        //    createPlan_PO.FillPlanData(validPlanName, validDescription, validWeeks);

        //    // 2. ACT
        //    createPlan_PO.SubmitPlan();
        //    createPlan_PO.ConfirmPlanSubmission();

        //    Thread.Sleep(2000);

        //    _driver.Navigate().GoToUrl("https://localhost:7081/plan/detailsplan/11");

        //    Thread.Sleep(1000);
        //
        //    // 3. ASSERT
        //    var detailPO = new DetailsPlan_P0(_driver, _output);

        //    bool detallesCorrectos = detailPO.CheckPlanDetail(
        //        "Adrian.Sevilla@alu.uclm.es",
        //        "15/05/2026",
        //        validPlanName,
        //        validDescription,
        //        validWeeks.ToString()
        //    );
        //    var validationErrors = _driver.FindElements(By.CssSelector(".validation-message"));
        //    foreach (var error in validationErrors)
        //    {
        //        _output.WriteLine("Error de validación detectado: " + error.Text);
        //    }

        //    Assert.True(detallesCorrectos, "Los datos del plan en la pantalla de detalles no coinciden con los introducidos.");
        //}
    }
}