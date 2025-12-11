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

        private const int itemId3 = 1;
        private const string itemName3 = "Resistance band set";
        private const string itemBrand3 = "Nike";
        private const string itemPrice3 = "22 €";
        private const string itemDescription3 = "Set of bands";
        private const string itemQuantity3 = "9";
        private const string itemAdd3 = "Add to cart (22 €)";

        private const int itemId1 = 2;
        private const string itemName1 = "Foam Roller";
        private const string itemBrand1 = "Adidas";
        private const string itemPrice1 = "25 €";
        private const string itemDescription1 = "Foam roller for muscle recovery and massage";
        private const string itemQuantity1 = "9";
        private const string itemAdd1 = "Add to cart (25 €)";


        private const int itemId2 = 3;
        private const string itemName2 = "Kettlebell 10 kg";
        private const string itemBrand2 = "Domyos";
        private const string itemPrice2 = "35 €";
        private const string itemDescription2 = "Ideal for strength and endurance training";
        private const string itemQuantity2 = "9";
        private const string itemAdd2 = "Add to cart (35 €)";

        private const string PurchaseId = "3";
        private const string quantityToBuy = "3";
        private const string totalPrice = "75 €";
        private const string UserEmail = "Adrian.Sevilla@alu.uclm.es";
        private const string UserPM = "Bizum";
        private const string UserStreet = "Calle Mayor";
        private const string UserCity = "Cuenca";
        private const string UserCountry = "Spain";




        public UC_PurchaseItem_UIT(ITestOutputHelper output) : base(output)
        {
            selectItemsforpurchase_P0 = new SelectItemsForPurchase_P0(_driver, _output);
        }
        private void Precondition_performance_login()
        {
            Perform_login("Adrian.Sevilla@alu.uclm.es", "Password123!");
        }
        private void InitialStepsForPurchaseItem()
        {
            Precondition_performance_login();
            selectItemsforpurchase_P0.WaitForBeingVisible(By.Id("SelectPurchase"));
            _driver.FindElement(By.Id("SelectPurchase")).Click();
        }
        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [InlineData(PurchaseId,itemName1,itemBrand1, itemPrice1,quantityToBuy, totalPrice, UserEmail, UserPM, UserStreet, UserCity, UserCountry,"")]
        public void UC8_Scen1_1_1_BasicFlow(string purchaseId,string itemName, string brand, string priceUnit, string quanityBuy, string expecectedTotalPrice, string email, string pM,string street,string city, string country,string description)
        {
            InitialStepsForPurchaseItem();
            var address = street + ", " + city + ", " + country;
            var expectedPurchaseDetails = new List<string[]>
            {
                new string[] { purchaseId, email,address,expecectedTotalPrice,description,pM,itemName,brand,quanityBuy,priceUnit }
            };  
        }

        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [InlineData(itemName1,itemBrand1,itemDescription1,itemPrice1,itemQuantity1,itemAdd1,"Foam Roller","")]
        [InlineData(itemName1,itemBrand1,itemDescription1,itemPrice1,itemQuantity1,itemAdd1,"","Adidas")]
        public void UC8_Scen3_1_2_Filtering(string name, string brand, string description, string price, string quantity, string add, string searchName, string searchBrand)
        {
            InitialStepsForPurchaseItem();
            var expectedItems = new List<string[]>
            {
                new string[] {name, brand,description, price, quantity, add }
            };
            selectItemsforpurchase_P0.SearchItems(searchName, searchBrand);
            selectItemsforpurchase_P0.Se

            Assert.True(selectItemsforpurchase_P0.CheckListOfItems(expectedItems));
        }

        //Testear no items available in the item1 
        [Fact(Skip ="first run the dto.Items")]
        //[Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_NoItemsAvailable()
        {
            InitialStepsForPurchaseItem();
            var expectedItems = new List<string[]>
            {
                  new string[]{ itemName1,itemBrand1, itemDescription1, itemPrice1, itemQuantity1, itemAdd1 },
                new string[]{ itemName2,itemBrand2, itemDescription2, itemPrice2, itemQuantity2, itemAdd2 }
              
            };   
            selectItemsforpurchase_P0.SearchItems("", "");

            Assert.True(selectItemsforpurchase_P0.CheckListOfItems(expectedItems));  
        }

    }
}
