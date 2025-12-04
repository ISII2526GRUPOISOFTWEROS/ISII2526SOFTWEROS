using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;


namespace AppForSEII2526.UIT.UC_Purchase
{
    public class UC_PurchaseItem_UIT : UC_UIT
    {
        private SelectItemsForPurchase_P0 selectItemsforpurchase_P0;
        private const int itemId1 = 2;
        private const string itemName1 = "Foam Roller";
        private const string itemBrand1 = "Adidas";
        private const string itemPrice1 = "25 €";
        private const string itemDescription1 = "Foam roller for muscle recovery and massage";
        private const string itemQuantity1 = "18";
        private const string itemAdd1 = "Add to cart (25 €)";


        private const int itemId2 = 3;
        private const string itemName2 = "Kettlebell 10 kg";
        private const string itemBrand2 = "Domyos";
        private const string itemPrice2 = "35 €";
        private const string itemDescription2 = "Ideal for strength and endurance training";
        private const string itemQuantity2 = "15";
        private const string itemAdd2 = "Add to cart (35€)";

        public UC_PurchaseItem_UIT(ITestOutputHelper output) : base(output)
        {
            selectItemsforpurchase_P0 = new SelectItemsForPurchase_P0(_driver, _output);
        }
        private void Precondition_performance_login()
        {
            Perform_login("adriansevillajimenez@gmail.com", "Adrian123!");
        }
        private void InitialStepsForPurchaseItem()
        {
            Precondition_performance_login();
            selectItemsforpurchase_P0.WaitForBeingVisible(By.Id("SelectPurchase"));
            _driver.FindElement(By.Id("SelectPurchase")).Click();
        }
        [Fact]
        [Trait("LevelTesting","Functional Testing")]
        public void UC8_Scen3_1_Filtering()
        {
            InitialStepsForPurchaseItem();
            var expectedItems = new List<string[]>
            {
                new string[] {itemName1, itemBrand1,itemDescription1, itemPrice1, itemQuantity1, itemAdd1}
            };
            selectItemsforpurchase_P0.SearchItems("Foam Roller", "");
            Assert.True(selectItemsforpurchase_P0.CheckListOfItems(expectedItems));
        }
        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [InlineData(itemName1,itemBrand1,itemDescription1,itemPrice1,itemQuantity1,itemAdd1,"Foam","")]
        public void UC8_Scen3_2_Filtering(string name, string brand, string description, string price, string quantity, string add, string searchName, string searchBrand)
        {
            InitialStepsForPurchaseItem();
            var expectedItems = new List<string[]>
            {
                new string[] {name, brand,description, price, quantity, add }
            };
            selectItemsforpurchase_P0.SearchItems(searchName, searchBrand);
            Assert.True(selectItemsforpurchase_P0.CheckListOfItems(expectedItems));
        }

    }
}
