using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Restock
{
    public class SelectItemsForRestock_P0 : PageObject
    {
        By inputName = By.Id("inputName");
        By inputQuantity = By.Id("inputQuantity");
        By buttonSearchItems = By.Id("searchItems");
        

        //By tableOfItemsBy = By.Id("TableOfItems");
        public SelectItemsForRestock_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchItems(string name, string quantity)
        { 
            WaitForBeingClickable(inputName);
            _driver.FindElement(inputName).SendKeys(name);


            WaitForBeingClickable(inputQuantity);
            _driver.FindElement(inputQuantity).SendKeys(quantity);

            _driver.FindElement(buttonSearchItems).Click();


            System.Threading.Thread.Sleep(500);

        }


        public void ClickRestockButton()
        {
            By buttonRestockItems = By.Id("createRestockButton");

            WaitForBeingClickable(buttonRestockItems);
            _driver.FindElement(buttonRestockItems).Click();
        }

        public void AddQuantityToItem(string itemName, string quantity)
        {
            By addItemButton = By.Id($"itemToRestock_{itemName}");
            WaitForBeingClickable(addItemButton);
         
                _driver.FindElement(addItemButton).Click();
                System.Threading.Thread.Sleep(200);

        }


        public void RemoveItemInCart(int id)
        {
            By removeItemButton = By.Id($"removeItem_{id}");
            WaitForBeingClickable(removeItemButton);
            _driver.FindElement(removeItemButton).Click();
        }
    }
}
