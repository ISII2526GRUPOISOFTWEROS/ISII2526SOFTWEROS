using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class CreatePlan_P0 : PageObject
    {
        By inputPlanName = By.Id("PlanName");
        By inputDescription = By.Id("Description");
        By inputWeeks = By.Id("NumberOfWeeks");
        By inputPaymentMethodId = By.Id("PaymentMethodId");
        By buttonModifyClasses = By.Id("buttonModify");
        By buttonCreatePlan = By.Id("Submit");
        By buttonDialogSave = By.Id("Button_DialogOK");
        By buttonDialogCancel = By.Id("Button_DialogCancel");
        By tableSelectedClasses = By.Id("TableOfSelectedClasses");

        public CreatePlan_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillPlanData(string planName, string description, int weeks, int paymentMethodId)
        {
            WaitForBeingClickable(inputPlanName);
            _driver.FindElement(inputPlanName).Clear();
            _driver.FindElement(inputPlanName).SendKeys(planName);

            if (!string.IsNullOrEmpty(description))
            {
                WaitForBeingClickable(inputDescription);
                _driver.FindElement(inputDescription).Clear();
                _driver.FindElement(inputDescription).SendKeys(description);
            }

            WaitForBeingClickable(inputWeeks);
            _driver.FindElement(inputWeeks).Clear();
            _driver.FindElement(inputWeeks).SendKeys(weeks.ToString());

            WaitForBeingClickable(inputPaymentMethodId);
            _driver.FindElement(inputPaymentMethodId).Clear();
            _driver.FindElement(inputPaymentMethodId).SendKeys(paymentMethodId.ToString());
        }

        public void ClickCreatePlan()
        {
            WaitForBeingClickable(buttonCreatePlan);
            _driver.FindElement(buttonCreatePlan).Click();
        }

        public void ClickModifyClasses()
        {
            WaitForBeingClickable(buttonModifyClasses);
            _driver.FindElement(buttonModifyClasses).Click();
        }

        public void ClickDialogSave()
        {
            WaitForBeingClickable(buttonDialogSave);
            _driver.FindElement(buttonDialogSave).Click();
        }

        public void ClickDialogCancel()
        {
            WaitForBeingClickable(buttonDialogCancel);
            _driver.FindElement(buttonDialogCancel).Click();
        }

        public void RemoveClass(int classId)
        {
            By removeButton = By.Id($"removeClass_{classId}");
            WaitForBeingClickable(removeButton);
            _driver.FindElement(removeButton).Click();
        }

        public bool CheckListOfClasses(List<string[]> expectedClasses)
        {
            return CheckBodyTable(expectedClasses, tableSelectedClasses);
        }
    }
}

