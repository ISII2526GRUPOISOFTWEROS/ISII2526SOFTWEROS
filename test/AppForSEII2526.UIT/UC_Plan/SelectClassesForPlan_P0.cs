using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class SelectClassesForPlan_PO : PageObject
    {
        By inputItemType = By.Id("inputTitle");
        By inputDate = By.Id("date");
        By inputFromDate = By.Id("fromDate");
        By inputToDate = By.Id("toDate");
        By buttonSearchClasses = By.Id("searchClasses");
        By tableAvailableClasses = By.Id("TableOfClasses");
        By proceedToPlanButton = By.Id("proceedToPlanButton");

        public SelectClassesForPlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchClasses(string itemType, DateTime? date, DateTime? fromDate, DateTime? toDate)
        {
            WaitForBeingClickable(inputItemType);
            var element = _driver.FindElement(inputItemType);
            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Backspace);
            element.SendKeys(itemType);
            element.SendKeys(Keys.Tab);

            if (date != null)
            {
                DateTime strictDate = (DateTime)date;
                InputDateInDatePicker(inputDate, strictDate);
            }

            if (fromDate != null)
            {
                DateTime strictFromDate = (DateTime)fromDate;
                InputDateInDatePicker(inputFromDate, strictFromDate);
            }

            if (toDate != null)
            {
                DateTime strictToDate = (DateTime)toDate;
                InputDateInDatePicker(inputToDate, strictToDate);
            }

            WaitForBeingClickable(buttonSearchClasses);
            _driver.FindElement(buttonSearchClasses).Click();

            System.Threading.Thread.Sleep(300); // Espera a que Blazor termine de refrescar la tabla de resultados
        }

        public bool CheckListOfClasses(List<string[]> expectedClasses)
        {
            return CheckBodyTable(expectedClasses, tableAvailableClasses);
        }

        public string GetErrorMessage()
        {
            By errorPanel = By.Id("errorsShown");

            try
            {
                WaitForBeingVisible(errorPanel);
                return _driver.FindElement(errorPanel).Text;
            }
            catch (WebDriverTimeoutException)
            {
                return "";
            }
        }

        public string GetWarningMessage()
        {
            By warningPanel = By.Id("warningsShown");

            try
            {
                WaitForBeingVisible(warningPanel);
                return _driver.FindElement(warningPanel).Text;
            }
            catch (WebDriverTimeoutException)
            {
                return "";
            }
        }

        public void PressProceedToPlanButton()
        {
            _output.WriteLine("Intentando pulsar el botón de Proceed...");

          
            By[] proceedSelectors = new By[] {
        By.Id("proceedToPlanButton"),
        By.XPath("//button[contains(text(), 'Proceed to Plan')]"),
        By.CssSelector(".btn-primary.w-100") 
    };

            IWebElement? element = null;

            for (int i = 0; i < 5; i++)
            {
                foreach (var selector in proceedSelectors)
                {
                    try
                    {
                        element = _driver.FindElement(selector);
                        if (element.Displayed && element.Enabled) break;
                    }
                    catch { continue; }
                }
                if (element != null && element.Enabled) break;
                System.Threading.Thread.Sleep(2000);
                _output.WriteLine($"reintentando encontrar botón Proceed ({i + 1}/5)");
            }

            if (element != null)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                js.ExecuteScript("arguments[0].click();", element);
                _output.WriteLine("Botón Proceed pulsado ");
            }
            else
            {
                throw new Exception("No se pudo encontrar o clicar el botón Proceed to Plan.");
            }
        }
        public void AddClassToSelected(string className)
        {
            By addButton = By.XPath($"//tr[td[contains(.,'{className}')]]//button");

            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var element = wait.Until(d => d.FindElement(addButton));

            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            js.ExecuteScript("arguments[0].click();", element);
        }
        public void RemoveClassFromSelected(int classId)
        {
            By removeButton = By.Id($"removeClass_{classId}");
            WaitForBeingClickable(removeButton);
            _driver.FindElement(removeButton).Click();
            System.Threading.Thread.Sleep(250); 
        }

        public bool IsProceedToPlanButtonVisible()
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
                return wait.Until(d => d.FindElement(proceedToPlanButton).Displayed);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}