using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class CreatePurchase_P0 : PageObject
    {
         By inputUserName = By.Id("CustomerUserName");
         By inputStreet = By.Id("Street");
         By inputCity = By.Id("City");
         By inputCountry = By.Id("Country");
         By inputDescription= By.Id("Description");
         private By selectPM= By.Id("PaymentMethod");
         private By buttonSubmit = By.Id("Submit");
         private By buttonSave = By.Id("Button_DialogOK");
         private By tableItems = By.Id("TableOfPurchaseItems");

        By buttonPurchaseItem = By.Id("purchaseItemButton");
        By buttonCancel = By.Id("buttonCancel");
        By buttonModifyItem = By.Id("buttonModify");


        public CreatePurchase_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillingDetails(string street, string city,string country,string description)
        { // wait for the web element to be clickable

            WaitForBeingClickable(inputStreet);
           _driver.FindElement(inputStreet).SendKeys(street);

            WaitForBeingClickable(inputCity);
           _driver.FindElement(inputCity).SendKeys(city);


            WaitForBeingClickable(inputCountry);
            _driver.FindElement(inputCountry).SendKeys(country);

            WaitForBeingClickable(inputDescription);
            _driver.FindElement(inputDescription).SendKeys(description);

            System.Threading.Thread.Sleep(500);
        }
        public void SelectPaymentMethod(string paymentMethodName)
        {

            WaitForBeingClickable(selectPM);
            var dropdown = _driver.FindElement(selectPM);

            var selectElement = new SelectElement(dropdown);
            selectElement.SelectByText(paymentMethodName);

            System.Threading.Thread.Sleep(500);

        }

        public void SubmitPurchase()
        {
            WaitForBeingClickable(buttonSubmit);
            _driver.FindElement(buttonSubmit).Click();

            System.Threading.Thread.Sleep(500);

            WaitForBeingClickable(buttonSave);
            _driver.FindElement(buttonSave).Click();

            System.Threading.Thread.Sleep(500);

        }
        public void ClickModifyItems()
        {
            WaitForBeingClickable(buttonModifyItem);
            _driver.FindElement(buttonModifyItem).Click();
        }
        public void ClickCancel()
        {
            WaitForBeingClickable(buttonCancel);
            _driver.FindElement(buttonCancel).Click();
        }
        
        public string GetErrorText()
        {
            try
            {

                var errorElement = _driver.FindElement(By.Id("ErrorShown"));
                if(errorElement.Displayed && !string.IsNullOrEmpty(errorElement.Text)){
                    return errorElement.Text;
                }
            }
            catch (NoSuchElementException) { }
            try
            {
                var summary = _driver.FindElement(By.CssSelector(".validation-summary-errors, .alert-danger ul"));
                if (summary.Displayed)
                {
                    return summary.Text;
                }
            }
            catch (NoSuchElementException) { }
            return string.Empty;
        }
    }
}
