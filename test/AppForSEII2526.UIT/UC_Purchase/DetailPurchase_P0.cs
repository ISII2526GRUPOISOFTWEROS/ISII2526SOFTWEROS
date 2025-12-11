using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class DetailPurchase_P0 : PageObject
    {
        private By labelPurchaseId = By.Id("PurchaseId");
        private By labelUserName = By.Id("UserName");
        private By labelAddress = By.Id("DeliveryAddress");
        private By labelTotalPrice = By.Id("TotalPriceHeader"); 
        private By labelDescription = By.Id("Description");
        private By labelPaymentMethod = By.Id("PaymentMethod");
        private By tableItems = By.Id("PurchaseItems");

        public DetailPurchase_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public string GetPurchaseId()
        {
            WaitForBeingClickable(labelPurchaseId);
            return _driver.FindElement(labelPurchaseId).Text;
        }
        public string GetPurchaseUser()
        {
            WaitForBeingClickable(labelUserName);
            return _driver.FindElement(labelUserName).Text;
        }
        public string GetPurchaseAddress()
        {
            WaitForBeingClickable(labelAddress);
            return _driver.FindElement(labelAddress).Text;
        }
        public string GetPurchaseTotalPrice()
        {
            WaitForBeingClickable(labelTotalPrice);
            return _driver.FindElement(labelTotalPrice).Text;
        }
        public string GetPurchasePaymentMethod()
        {
            WaitForBeingClickable(labelPaymentMethod);
            return _driver.FindElement(labelPaymentMethod).Text;
        }
        public string GetPurchaseDescription()
        {
            WaitForBeingClickable(labelDescription);
            return _driver.FindElement(labelDescription).Text;
        }
        public bool IsItemInTable(string itemName, int expectedQuanity)
        {
            By rowLocator = By.Id($"PurchaseItem_{itemName}");
            try
            {
                WaitForBeingVisible(rowLocator);
                return _driver.FindElement(rowLocator).Text.Contains(expectedQuanity.ToString());
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
