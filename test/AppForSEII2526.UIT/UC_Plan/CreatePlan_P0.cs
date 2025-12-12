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
        By inputPlanName = By.XPath("//label[contains(text(),'Plan Name')]/following-sibling::input");
        By inputDescription = By.XPath("//label[contains(text(),'Description')]/following-sibling::input");
        By inputWeeks = By.XPath("//label[contains(text(),'Weeks')]/following-sibling::input");
        By inputPaymentMethodId = By.XPath("//label[contains(text(),'Payment Method ID')]/following-sibling::input");
        By buttonModifyClasses = By.XPath("//button[contains(text(),'Add / Modify Classes')]");
        By buttonCreatePlan = By.XPath("//button[@type='submit' and contains(text(),'Create Plan')]");
        By tableSelectedClasses = By.XPath("//h4[contains(text(),'Selected Classes')]/following-sibling::div//table");
        By totalPriceDisplay = By.XPath("//b[contains(text(),'Total Price:')]/parent::p");
        By errorAlert = By.XPath("//div[contains(@class,'alert-danger')]");
        By validationSummary = By.XPath("//div[contains(@class,'validation-summary')]");
        By buttonDialogSave = By.XPath("//button[contains(text(),'Save') or @id='Button_DialogOK']");
        By buttonDialogCancel = By.XPath("//button[contains(text(),'Cancel') or contains(text(),'Not')]");

        public CreatePlan_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillPlanData(string planName, string description, int weeks, int paymentMethodId)
        {
            WaitForBeingVisible(inputPlanName);

            _driver.FindElement(inputPlanName).Clear();
            _driver.FindElement(inputPlanName).SendKeys(planName);

            if (!string.IsNullOrEmpty(description))
            {
                _driver.FindElement(inputDescription).Clear();
                _driver.FindElement(inputDescription).SendKeys(description);
            }

            _driver.FindElement(inputWeeks).Clear();
            _driver.FindElement(inputWeeks).SendKeys(weeks.ToString());

            _driver.FindElement(inputPaymentMethodId).Clear();
            _driver.FindElement(inputPaymentMethodId).SendKeys(paymentMethodId.ToString());
        }

        public void ClickCreatePlan()
        {
            WaitForBeingClickable(buttonCreatePlan);
            _driver.FindElement(buttonCreatePlan).Click();
            Thread.Sleep(500);
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
            Thread.Sleep(1000);
        }

        public void ClickDialogCancel()
        {
            WaitForBeingClickable(buttonDialogCancel);
            _driver.FindElement(buttonDialogCancel).Click();
            Thread.Sleep(500);
        }

        public void RemoveClass(string className)
        {
            var removeButton = By.XPath($"//td[contains(text(),'{className}')]/following-sibling::td//button[contains(text(),'Remove')]");
            WaitForBeingClickable(removeButton);
            _driver.FindElement(removeButton).Click();
            Thread.Sleep(500);
        }

        public bool CheckMessageError(string errorMessage)
        {
            try
            {
                IWebElement actualErrorShown = _driver.FindElement(errorAlert);
                _output.WriteLine($"Actual Message shown: {actualErrorShown.Text}");
                return actualErrorShown.Text.Contains(errorMessage);
            }
            catch
            {
                return false;
            }
        }

        public bool IsDialogDisplayed()
        {
            try
            {
                WaitForBeingVisible(buttonDialogSave);
                return _driver.FindElement(buttonDialogSave).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidationDisplayed()
        {
            try
            {
                return _driver.FindElement(validationSummary).Displayed ||
                       _driver.FindElement(errorAlert).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public int GetSelectedClassesCount()
        {
            try
            {
                WaitForBeingVisible(tableSelectedClasses);
                var rows = _driver.FindElement(tableSelectedClasses)
                    .FindElement(By.TagName("tbody"))
                    .FindElements(By.TagName("tr"));
                return rows.Count;
            }
            catch
            {
                return 0;
            }
        }

        public decimal GetTotalPrice()
        {
            try
            {
                WaitForBeingVisible(totalPriceDisplay);
                var text = _driver.FindElement(totalPriceDisplay).Text;
                var match = System.Text.RegularExpressions.Regex.Match(text, @"(\d+(?:\.\d+)?)");
                if (match.Success)
                {
                    return decimal.Parse(match.Groups[1].Value);
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public bool CheckListOfClasses(List<string[]> expectedClasses)
        {
            return CheckBodyTable(expectedClasses, tableSelectedClasses);
        }
    }
}
