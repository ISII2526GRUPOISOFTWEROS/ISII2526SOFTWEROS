using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class SelectItemsForPurchase_P0 : PageObject
    {
         By inputName = By.Id("inputName");
         By inputBrand = By.Id("inputBrand");
         By buttonSearchItems = By.Id("searchItems");
         By buttonPurchaseItems = By.Id("purchaseItemButton");
        By tableOfItemsBy = By.Id("TableOfItems");

        public SelectItemsForPurchase_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchItems(string name, string brand)
        { // wait for the web element to be clickable
            WaitForBeingClickable(inputName);
           _driver.FindElement(inputName).SendKeys(name);


            WaitForBeingClickable(inputBrand);
           _driver.FindElement(inputBrand).SendKeys(brand);

            WaitForBeingClickable(buttonSearchItems);
            _driver.FindElement(buttonSearchItems).Click();

            System.Threading.Thread.Sleep(500);
        }

        public bool CheckListOfItems(List<string[]> expectedItems)
        {
            return CheckBodyTable(expectedItems, tableOfItemsBy);
        }
        public void ClickPurchaseButton()
        {
            WaitForBeingClickable(buttonPurchaseItems);
            _driver.FindElement(buttonPurchaseItems).Click();
        }
        public void AddQuantityToItem(string itemName, int quantity)
        {
            By addItemButton = By.Id($"itemforpurchase_{itemName}");
            WaitForBeingClickable(addItemButton);

            for (int i = 0; i < quantity; i++) { 
                _driver.FindElement(addItemButton).Click();
                System.Threading.Thread.Sleep(200);
            }

        }
    }
}
