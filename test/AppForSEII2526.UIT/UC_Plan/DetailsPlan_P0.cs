using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class DetailsPlan_P0 : PageObject
    {
        By nameSurname = By.Id("nameSurname");
        By createdDate = By.Id("createdDate");
        By planName = By.Id("planName");
        By planDescription = By.Id("planDescription");
        By weeks = By.Id("weeks");
        By tableofClassesBy = By.Id("enrolledClasses");

        public DetailsPlan_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckPlanDetail(string nameSurnameText, string createdDateText, string planNameText,
            string planDescriptionText, string weeksText)
        {
            WaitForBeingVisible(nameSurname);
            WaitForBeingVisible(createdDate);
            WaitForBeingVisible(planName);
            WaitForBeingVisible(planDescription);
            WaitForBeingVisible(weeks);

            bool result = true;

            result = result && _driver.FindElement(nameSurname).Text.Contains(nameSurnameText);
            result = result && _driver.FindElement(createdDate).Text.Contains(createdDateText);
            result = result && _driver.FindElement(planName).Text.Contains(planNameText);
            result = result && _driver.FindElement(planDescription).Text.Contains(planDescriptionText);
            result = result && _driver.FindElement(weeks).Text.Contains(weeksText);

            return result;
        }

        public bool CheckListOfItems(List<string[]> expectedItems)
        {
            return CheckBodyTable(expectedItems, tableofClassesBy);
        }
    }
}