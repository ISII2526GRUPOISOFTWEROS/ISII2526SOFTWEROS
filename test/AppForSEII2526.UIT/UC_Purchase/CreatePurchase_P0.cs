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

        By errorShown = By.Id("ErrorShown");
        By buttonCancel = By.Id("buttonCancel");
        By buttonModifyItem = By.Id("buttonModify");


        public CreatePurchase_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SetUserName(string userName)
        {
            WaitForBeingClickable(By.Id("CustomerUserName"));
            var userField = _driver.FindElement(By.Id("CustomerUserName"));
            userField.Clear();
            userField.SendKeys(userName);
            userField.SendKeys(Keys.Tab); 
        }

        public void FillingDetails(string street, string city,string country,string description)
        { 

            WaitForBeingClickable(inputStreet);
           _driver.FindElement(inputStreet).SendKeys(street);

            WaitForBeingClickable(inputCity);
           _driver.FindElement(inputCity).SendKeys(city);


            WaitForBeingClickable(inputCountry);
            _driver.FindElement(inputCountry).SendKeys(country);

            WaitForBeingClickable(inputDescription);
            _driver.FindElement(inputDescription).SendKeys(description);

        }
        public void SelectPaymentMethod(string paymentMethodName)
        {

            WaitForBeingClickable(selectPM);
            var dropdown = _driver.FindElement(selectPM);

            var selectElement = new SelectElement(dropdown);
            selectElement.SelectByText(paymentMethodName);


        }

        public void SubmitPurchase()
        {
            WaitForBeingClickable(buttonSubmit);
            _driver.FindElement(buttonSubmit).Click();
        }
        public void ConfirmPurchase()
        {
            WaitForBeingClickable(buttonSave);
            _driver.FindElement(buttonSave).Click();


        }
        public void ClickModifyItems()
        {
            WaitForBeingClickable(buttonModifyItem);
            _driver.FindElement(buttonModifyItem).Click();
        }
      
        public void SetItemQuanity(int itemId, string quanity)
        {
            string rowId = $"ItemData_{itemId}";
            var quantityInput = _driver.FindElement(By.CssSelector($"tr#{rowId} input"));

            WaitForBeingClickable(By.CssSelector($"tr#{rowId} input"));
            quantityInput.Clear();
            quantityInput.SendKeys(quanity);
            quantityInput.SendKeys(Keys.Tab);

            System.Threading.Thread.Sleep(500);
        }
        public bool IsItemInSummary(int itemId)
        {
            try
            {
                WebDriverWait shortWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(1));
                var row = shortWait.Until(d => d.FindElement(By.Id($"ItemData_{itemId}")));
                return row.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            } 
            catch(NoSuchElementException)
            {
                return false;
            }
        }
        public string GetErrorText()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));

            try
            {
                return wait.Until(d =>
                {
                    try
                    {
                        var serverError = d.FindElement(By.Id("ErrorShown"));
                        if (serverError.Displayed && !string.IsNullOrWhiteSpace(serverError.Text))
                        {
                            return serverError.Text;
                        }
                    }
                    catch (StaleElementReferenceException) { }
                    catch (NoSuchElementException) { }

                    try
                    {
                        var clientError = d.FindElement(By.CssSelector(".alert.alert-danger"));
                        if (clientError.Displayed && !string.IsNullOrWhiteSpace(clientError.Text))
                        {
                            return clientError.Text;
                        }
                    }
                    catch (StaleElementReferenceException) { }
                    catch (NoSuchElementException) { }

                    return null;
                });
            }
            catch (WebDriverTimeoutException)
            {
                return string.Empty;
            }
        }
    }
}
