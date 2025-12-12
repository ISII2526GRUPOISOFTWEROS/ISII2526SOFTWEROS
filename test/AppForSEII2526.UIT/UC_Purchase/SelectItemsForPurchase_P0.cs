//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AppForSEII2526.UIT.UC_Purchase
//{
//    public class SelectItemsForPurchase_P0 : PageObject
//    {
//         By inputName = By.Id("inputName");
//         By inputBrand = By.Id("inputBrand");
//         By buttonSearchItems = By.Id("searchItems");
//        By tableOfItemsBy = By.Id("TableOfItems");
//        public SelectItemsForPurchase_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
//        {
//        }
//        public void SearchItems(string name, string brand)
//        { // wait for the web element to be clickable
//            WaitForBeingClickable(inputName);
//            _driver.FindElement(inputName).SendKeys(name);


//            WaitForBeingClickable(inputBrand);
//            _driver.FindElement(inputBrand).SendKeys(brand);

//            _driver.FindElement(buttonSearchItems).Click();
//        }

//        public bool CheckListOfItems(List<string[]> expectedItems)
//        {
//            return CheckBodyTable(expectedItems, tableOfItemsBy);
//        }
//    }
//}
