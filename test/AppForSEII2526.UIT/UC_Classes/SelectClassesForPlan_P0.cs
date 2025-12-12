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
        By buttonSearchClasses = By.Id("searchClasses");
        By buttonProceedToPlan = By.Id("proceedToPlanButton");
        By tableAvailableClasses = By.Id("TableOfClasses");

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

            WaitForBeingClickable(buttonSearchClasses);
            _driver.FindElement(buttonSearchClasses).Click();
            System.Threading.Thread.Sleep(500);
        }

        public bool CheckListOfClasses(List<string[]> expectedClasses)
        {
            return CheckBodyTable(expectedClasses, tableAvailableClasses);
        }

        public void ClickProceedToPlan()
        {
            WaitForBeingClickable(buttonProceedToPlan);
            _driver.FindElement(buttonProceedToPlan).Click();
        }

        public void AddClassToCart(string className)
        {
            By addClassButton = By.Id($"classforplan_{className}");
            WaitForBeingClickable(addClassButton);
            _driver.FindElement(addClassButton).Click();
            System.Threading.Thread.Sleep(200);
        }

        public void RemoveClassFromCart(int classId)
        {
            By removeClassButton = By.Id($"removeClass_{classId}");
            WaitForBeingClickable(removeClassButton);
            _driver.FindElement(removeClassButton).Click();
        }
    }
}


