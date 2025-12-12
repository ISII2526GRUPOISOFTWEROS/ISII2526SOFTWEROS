using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Classes
{
    public class SelectClassesForPlan_P0 : PageObject
    {
        By inputItemType = By.Id("inputTitle");
        By inputDate = By.Id("date");
        By inputFromDate = By.Id("fromDate");
        By inputToDate = By.Id("toDate");
        By buttonSearchClasses = By.XPath("//button[contains(text(),'Search Classes')]");
        By tableAvailableClasses = By.XPath("//h4[contains(text(),'Available Classes')]/following-sibling::div//table");
        By noClassesMessage = By.XPath("//p[contains(text(),'No classes found')]");
        By totalPriceDisplay = By.XPath("//b[contains(text(),'Total:')]/parent::p");
        By buttonProceedToPlan = By.XPath("//a[contains(text(),'Proceed to Plan')]");
        By errorAlert = By.XPath("//div[contains(@class,'alert-danger')]");

        public SelectClassesForPlan_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchClasses(string itemType, DateTime? date, DateTime? fromDate, DateTime? toDate)
        {
            WaitForBeingClickable(inputItemType);

            if (!string.IsNullOrEmpty(itemType))
            {
                _driver.FindElement(inputItemType).Clear();
                _driver.FindElement(inputItemType).SendKeys(itemType);
            }

            if (date.HasValue)
            {
                InputDateInDatePicker(inputDate, date.Value);
            }

            if (fromDate.HasValue)
            {
                InputDateInDatePicker(inputFromDate, fromDate.Value);
            }

            if (toDate.HasValue)
            {
                InputDateInDatePicker(inputToDate, toDate.Value);
            }

            _driver.FindElement(buttonSearchClasses).Click();
            Thread.Sleep(1000);
        }

        public bool CheckListOfClasses(List<string[]> expectedClasses)
        {
            return CheckBodyTable(expectedClasses, tableAvailableClasses);
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

        public void AddClassToCart(string className)
        {
            var addButton = By.XPath($"//td[contains(text(),'{className}')]/following-sibling::td//button[contains(text(),'Add')]");
            WaitForBeingClickable(addButton);
            _driver.FindElement(addButton).Click();
            Thread.Sleep(500);
        }

        public void RemoveClassFromCart(string className)
        {
            var removeButton = By.XPath($"//h6[contains(text(),'{className}')]/parent::div/following-sibling::button[contains(text(),'X')]");
            WaitForBeingClickable(removeButton);
            _driver.FindElement(removeButton).Click();
            Thread.Sleep(500);
        }

        public bool ProceedButtonNotAvailable()
        {
            try
            {
                return _driver.FindElement(buttonProceedToPlan).Displayed == false;
            }
            catch
            {
                return true;
            }
        }

        public bool NoClassesMessageDisplayed()
        {
            try
            {
                WaitForBeingVisible(noClassesMessage);
                return _driver.FindElement(noClassesMessage).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public int GetAvailableClassesCount()
        {
            try
            {
                WaitForBeingVisible(tableAvailableClasses);
                var rows = _driver.FindElement(tableAvailableClasses)
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

        public void ClickProceedToPlan()
        {
            WaitForBeingClickable(buttonProceedToPlan);
            _driver.FindElement(buttonProceedToPlan).Click();
        }
    }
}
