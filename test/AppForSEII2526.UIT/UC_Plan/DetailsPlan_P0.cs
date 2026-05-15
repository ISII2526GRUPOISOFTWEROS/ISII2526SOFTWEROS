using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
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
        private By labelTotalPrice = By.Id("TotalPrice");
        private By tableClasses = By.Id("PlanClasses");

        public DetailsPlan_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public string GetPlanName()
        {
            WaitForBeingVisible(labelPlanName); // Asegurar que Blazor ha pintado el texto
            return _driver.FindElement(labelPlanName).Text;
        }

        public string GetPlanUsername()
        {
            WaitForBeingVisible(labelUsername);
            return _driver.FindElement(labelUsername).Text;
        }

        public string GetPlanDescription()
        {
            WaitForBeingVisible(labelDescription);
            return _driver.FindElement(labelDescription).Text;
        }

        public string GetPlanCreatedDate()
        {
            WaitForBeingVisible(labelCreatedDate);
            return _driver.FindElement(labelCreatedDate).Text;
        }

        public string GetPlanWeeks()
        {
            WaitForBeingVisible(labelWeeks);
            return _driver.FindElement(labelWeeks).Text;
        }

        public string GetPlanTotalPrice()
        {
            WaitForBeingVisible(labelTotalPrice);
            return _driver.FindElement(labelTotalPrice).Text;
        }

        public bool IsClassInTable(string className)
        {
            By rowLocator = By.Id($"PlanClass_{className}");
            try
            {
                WaitForBeingVisible(rowLocator);
                return _driver.FindElement(rowLocator).Displayed;
            }
            catch (Exception) // Captura general para evitar fallos por desincronización
            {
                return false;
            }
        }
    }
}