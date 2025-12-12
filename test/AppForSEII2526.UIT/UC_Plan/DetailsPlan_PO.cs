using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class DetailsPlanPO : PageObject
    {
        By planNameHeader = By.XPath("//div[contains(@class,'card-header')]//h4");
        By usernameCell = By.XPath("//th[contains(text(),'Username')]/following-sibling::td");
        By descriptionCell = By.XPath("//th[contains(text(),'Description')]/following-sibling::td");
        By createdDateCell = By.XPath("//th[contains(text(),'Created Date')]/following-sibling::td");
        By weeksCell = By.XPath("//th[contains(text(),'Number of Weeks')]/following-sibling::td");
        By healthIssuesCell = By.XPath("//th[contains(text(),'Health Issues')]/following-sibling::td");
        By totalPriceCell = By.XPath("//th[contains(text(),'Total Price')]/following-sibling::td");
        By classesTable = By.XPath("//h4[contains(text(),'Selected Classes')]/following-sibling::div//table");
        By errorAlert = By.XPath("//div[contains(@class,'alert-danger')]");

        public DetailsPlanPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
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

        public bool IsPlanDisplayed()
        {
            try
            {
                WaitForBeingVisible(planNameHeader);
                return _driver.FindElement(planNameHeader).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public string GetPlanName()
        {
            WaitForBeingVisible(planNameHeader);
            return _driver.FindElement(planNameHeader).Text;
        }

        public string GetUsername()
        {
            WaitForBeingVisible(usernameCell);
            return _driver.FindElement(usernameCell).Text;
        }

        public string GetDescription()
        {
            try
            {
                return _driver.FindElement(descriptionCell).Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetCreatedDate()
        {
            WaitForBeingVisible(createdDateCell);
            return _driver.FindElement(createdDateCell).Text;
        }

        public int GetNumberOfWeeks()
        {
            WaitForBeingVisible(weeksCell);
            var text = _driver.FindElement(weeksCell).Text;
            return int.Parse(text);
        }

        public string GetHealthIssues()
        {
            try
            {
                return _driver.FindElement(healthIssuesCell).Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetTotalPrice()
        {
            WaitForBeingVisible(totalPriceCell);
            return _driver.FindElement(totalPriceCell).Text;
        }

        public bool CheckListOfClasses(List<string[]> expectedClasses)
        {
            return CheckBodyTable(expectedClasses, classesTable);
        }

        public int GetClassesCount()
        {
            try
            {
                WaitForBeingVisible(classesTable);
                var rows = _driver.FindElement(classesTable)
                    .FindElement(By.TagName("tbody"))
                    .FindElements(By.TagName("tr"));
                return rows.Count;
            }
            catch
            {
                return 0;
            }
        }

        public bool CheckPlanData(string expectedName, string expectedUsername, int expectedWeeks)
        {
            try
            {
                bool nameMatch = GetPlanName().Equals(expectedName);
                bool usernameMatch = GetUsername().Contains(expectedUsername);
                bool weeksMatch = GetNumberOfWeeks() == expectedWeeks;

                _output.WriteLine($"Name match: {nameMatch}, Username match: {usernameMatch}, Weeks match: {weeksMatch}");

                return nameMatch && usernameMatch && weeksMatch;
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error checking plan data: {ex.Message}");
                return false;
            }
        }
    }
}
