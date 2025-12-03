using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class SelectItemsForPurchase_P0 : PageObject
    {
        private By inputName = By.Id("inputName");
        private By inputBrand = By.Id("inputBrand");
        private By buttonSearchItems = By.Id("searchItems");
        protected SelectItemsForPurchase_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void SearchItems(string name)
        { // wait for the web element to be clickable
            WaitForBeingClickable(inputName);
            _driver.FindElement(inputName).SendKeys(name);
            _driver.FindElement(buttonSearchItems).Click();
        }
    }
}
