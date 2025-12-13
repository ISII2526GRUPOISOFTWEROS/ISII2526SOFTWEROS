using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Restock
{
    public class DetailRestock_PO : PageObject
    {
        // ----- General info -----
        private By labelRestockTitle = By.Id("RestockTitle");
        private By labelResponsible = By.Id("RestockResponsible");
        private By labelAddress = By.Id("DeliveryAddress");
        private By labelDescription = By.Id("Description");
        private By labelDates = By.Id("RestockDates");
        private By labelTotalPrice = By.Id("TotalPrice");

        // ----- Items table -----
        private By tableItems = By.Id("RestockItems");

        public DetailRestock_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        // ---------- Getters ----------

        public string GetRestockTitle()
        {
            WaitForBeingVisible(labelRestockTitle);
            return _driver.FindElement(labelRestockTitle).Text;
        }

        public string GetRestockResponsible()
        {
            WaitForBeingVisible(labelResponsible);
            return _driver.FindElement(labelResponsible).Text;
        }

        public string GetDeliveryAddress()
        {
            WaitForBeingVisible(labelAddress);
            return _driver.FindElement(labelAddress).Text;
        }

        public string GetDescription()
        {
            WaitForBeingVisible(labelDescription);
            return _driver.FindElement(labelDescription).Text;
        }

        public string GetRestockDates()
        {
            WaitForBeingVisible(labelDates);
            return _driver.FindElement(labelDates).Text;
        }

        public string GetTotalPrice()
        {
            WaitForBeingVisible(labelTotalPrice);
            return _driver.FindElement(labelTotalPrice).Text;
        }

        public bool CheckItemsList(List<string[]> expectedItems)
        {
            return CheckBodyTable(expectedItems, tableItems);
        }

        // ---------- Table validation ----------

        public bool IsItemInTable(string itemName, int expectedQuantity)
        {
            By rowLocator = By.Id($"RestockItem_{itemName}");

            try
            {
                WaitForBeingVisible(rowLocator);
                var rowText = _driver.FindElement(rowLocator).Text;
                return rowText.Contains(expectedQuantity.ToString());
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
