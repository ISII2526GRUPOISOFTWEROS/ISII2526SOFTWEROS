using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class DetailsPlan_P0 : PageObject
    {
        private By labelPlanName = By.Id("PlanName");
        private By labelUsername = By.Id("Username");
        private By labelDescription = By.Id("Description");
        private By labelCreatedDate = By.Id("CreatedDate");
        private By labelWeeks = By.Id("NumberOfWeeks");
        private By labelHealthIssues = By.Id("HealthIssues");
        private By labelTotalPrice = By.Id("TotalPrice");
        private By tableClasses = By.Id("PlanClasses");

        public DetailsPlan_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public string GetPlanName()
        {
            WaitForBeingClickable(labelPlanName);
            return _driver.FindElement(labelPlanName).Text;
        }

        public string GetPlanUsername()
        {
            WaitForBeingClickable(labelUsername);
            return _driver.FindElement(labelUsername).Text;
        }

        public string GetPlanDescription()
        {
            WaitForBeingClickable(labelDescription);
            return _driver.FindElement(labelDescription).Text;
        }

        public string GetPlanCreatedDate()
        {
            WaitForBeingClickable(labelCreatedDate);
            return _driver.FindElement(labelCreatedDate).Text;
        }

        public string GetPlanWeeks()
        {
            WaitForBeingClickable(labelWeeks);
            return _driver.FindElement(labelWeeks).Text;
        }

        public string GetPlanHealthIssues()
        {
            WaitForBeingClickable(labelHealthIssues);
            return _driver.FindElement(labelHealthIssues).Text;
        }

        public string GetPlanTotalPrice()
        {
            WaitForBeingClickable(labelTotalPrice);
            return _driver.FindElement(labelTotalPrice).Text;
        }

        public bool IsClassInTable(string className, int expectedQuantity)
        {
            By rowLocator = By.Id($"PlanClass_{className}");
            try
            {
                WaitForBeingVisible(rowLocator);
                return _driver.FindElement(rowLocator).Text.Contains(expectedQuantity.ToString());
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}

