using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace AppForSEII2526.UIT.UC_Restock
{
    public class CreateRestock_PO: PageObject
    {
        By inputTitle = By.Id("Title");
        By inputDeliveryAddress = By.Id("DeliveryAddress");
        By inputDescription = By.Id("Description");
        By inputExpectedDate = By.Id("ExpectedDate");
        By inputResponsible = By.Id("Responsible");
        By buttonSubmit = By.Id("Submit");
        By buttonModifyItems = By.Id("ModifyItems");
        By tableRestockItems = By.Id("TableOfRestockItems");
        By errorBox = By.Id("ErrorsShown");
        By confirmRestockButton = By.Id("Button_DialogOK");
        By validationMessages = By.ClassName("validation-message");



        public CreateRestock_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public void FillForm(string title, string address, string description, string restockResponsible)
        {
            WaitForBeingClickable(inputTitle);
            _driver.FindElement(inputTitle).SendKeys(title);

            WaitForBeingClickable(inputDeliveryAddress);
            _driver.FindElement(inputDeliveryAddress).SendKeys(address);

            WaitForBeingClickable(inputDescription);
            _driver.FindElement(inputDescription).SendKeys(description);

            WaitForBeingClickable(inputResponsible);
            _driver.FindElement(inputResponsible).SendKeys(restockResponsible);
        }

        public void SetExpectedDate(DateTime date)
        {
            WaitForBeingClickable(inputExpectedDate);
            _driver.FindElement(inputExpectedDate);


            _driver.FindElement(inputExpectedDate).SendKeys(date.ToString("dd-MM-yyyy"));
        }

        public void Submit()
        {
            WaitForBeingClickable(buttonSubmit);
            _driver.FindElement(buttonSubmit).Click();
            Thread.Sleep(300);
        }

        public void OpenModifyItems()
        {
            WaitForBeingClickable(buttonModifyItems);
            _driver.FindElement(buttonModifyItems).Click();
            Thread.Sleep(300);
        }

        public void confirmRestockOrder()
        {
            WaitForBeingClickable(confirmRestockButton);
            _driver.FindElement(confirmRestockButton).Click();
            Thread.Sleep(300);
        }

        public bool HasErrors()
        {
            try
            {
                var text = _driver.FindElement(errorBox).Text;
                return !string.IsNullOrWhiteSpace(text);
            }
            catch
            {
                return false;
            }
        }

        public bool TableHasItem(string itemName)
        {
            var row = By.Id($"ItemData_{itemName}");
            return true;
        }

        public string GetErrorText()
        {
            WaitForBeingVisible(errorBox);
            return _driver.FindElement(errorBox).Text;
        }

        public string GetValidationErrors()
        {
            WaitForBeingVisible(By.ClassName("validation-message"));
            return string.Join(
                " ",
                _driver.FindElements(By.ClassName("validation-message"))
                       .Select(e => e.Text)
            );
        }

    }
}
