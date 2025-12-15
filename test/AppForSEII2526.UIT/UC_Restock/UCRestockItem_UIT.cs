using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_Purchase;
using AppForSEII2526.UIT.UC_Restock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace AppForSEII2526.UIT.UC_Restock
{
    public class UCRestockItem_UIT : UC_UIT
    {
        private SelectItemsForRestock_P0 selectItemsForRestock_P0;

        private const int itemId1 = 12;
        private const string itemName1 = "Foam Roller";
        private const string itemBrand1 = "Adidas";
        private const string itemQuantityRestock1 = "18";
        private const string itemQuantity1 = "1";
        private const string itemPrice1 = "8 €";
        private const string itemPriceNumber1 = "8";

        private const int itemId2 = 14;
        private const string itemName2 = "Kettlebell 10 kg";
        private const string itemBrand2 = "Domyos";
        private const string itemQuantityRestock2 = "15";
        private const string itemQuantity2 = "1";
        private const string itemPrice2 = "5 €";
        private const string itemPriceNumber2 = "5";


        private const string title = "Restock Order 1";
        private const string deliveryaddress = "Muy lejos";
        private const string description = "Restock for testing";
        private const string userName = "test@gmail.com";

        private const string itemName_wrong = "Protein";
        private const string itemQuantityRestock_wrong = "1";
        private const string itemQuantity_wrong = "1";


        public UCRestockItem_UIT(ITestOutputHelper output) : base(output)
        {
            selectItemsForRestock_P0 = new SelectItemsForRestock_P0(_driver, _output);

        }


        private void Precondition_performance_login()
        {
            Perform_login("test@gmail.com", "Password123!");
            System.Threading.Thread.Sleep(500);

        }

        private void InitialStepsForRestockItem()
        {
            Precondition_performance_login();
            selectItemsForRestock_P0.WaitForBeingVisible(By.Id("SelectRestock"));
            _driver.FindElement(By.Id("SelectRestock")).Click();
        }

        [Fact]
        [Trait("Level Testing", "Functional Testing")]
        public void UC7_Scen1_RestockItem_SuccessfulRestock()
        {
            var selectItemPO = new SelectItemsForRestock_P0(_driver, _output);
            var createRestockOrderPO = new CreateRestock_PO(_driver, _output);
            var DetailRestockOrderPO = new DetailRestock_PO(_driver, _output);
            var expectedItems = new List<string[]>
            {
                new string[] { itemName1, itemQuantity1, itemPrice1, itemPrice1 }
            };


            InitialStepsForRestockItem();
            selectItemPO.SearchItems(itemName1, itemQuantityRestock1);
            selectItemPO.AddQuantityToItem(itemName1, itemQuantityRestock1);
            selectItemPO.ClickRestockButton();

            createRestockOrderPO.FillForm("Restock Order 1", "Muy lejos", "Restock for testing ", "test@gmail.com");

            createRestockOrderPO.SetExpectedDate(DateTime.Now.AddDays(10));

            createRestockOrderPO.Submit();

            createRestockOrderPO.confirmRestockOrder();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("restock/detailrestock"));

            //Assert
            Assert.Contains("restock/detailrestock", _driver.Url);
            Assert.True(DetailRestockOrderPO.CheckItemsList(expectedItems));

        }

        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [InlineData("Foam Roller", "1")]
        [InlineData("Protein", "")]
        [InlineData("", "-1")]
        public void UC7_Scen2_RestockNoAvailable(string filterName, string filterQuantity)
        {

            InitialStepsForRestockItem();

            selectItemsForRestock_P0.SearchItems(filterName, filterQuantity);

            // Assert → no debe haber items
            bool hasItems = selectItemsForRestock_P0.HasAnyItems();
            Assert.False(hasItems);
        }

        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [InlineData(itemName1, itemBrand1, "18", itemPriceNumber1)]
        [InlineData(itemName2, itemBrand2, "15", itemPriceNumber2)]
        public void UC7_Scen3_FilteringItems(string filterName, string Brand, string filterQuantity, string value)
        {
            var expectedItems = new List<string[]>
            {
                new string[] {filterName, Brand, value, "Add" }
            };

            InitialStepsForRestockItem();

            selectItemsForRestock_P0.SearchItems(filterName, filterQuantity);

            Assert.True(selectItemsForRestock_P0.CheckItemsList(expectedItems));

        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]

        public void UC7_Scen6_ModifyCart()
        {

            var selectItemPO = new SelectItemsForRestock_P0(_driver, _output);
            var createRestockOrderPO = new CreateRestock_PO(_driver, _output);


            InitialStepsForRestockItem();
            selectItemPO.SearchItems(itemName1, itemQuantityRestock1);
            selectItemPO.AddQuantityToItem(itemName1, itemQuantityRestock1);

            selectItemPO.SearchItems(itemName2, itemQuantityRestock2);
            selectItemPO.AddQuantityToItem(itemName2, itemQuantityRestock2);

            selectItemPO.ClickRestockButton();


            createRestockOrderPO.OpenModifyItems();
            selectItemPO.RemoveItemInCart(itemId1);

            selectItemPO.ClickRestockButton();


            Assert.True(createRestockOrderPO.TableHasItem(itemName2));

        }

        public static IEnumerable<object[]> GetRestockValidationScenarios()
        {
            return new List<object[]>
    {
       
        // Description no empieza por "Restock for"
        new object[]
        {
            "Restock Order 2", "Warehouse 1", "Buy items", "",
            "Errors: (*) Description: Error! You must start the Description with 'Restock for'"
        }
    };
        }

        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [MemberData(nameof(GetRestockValidationScenarios))]
        public void UC7_Scen5_ValidationErrors_Restock(
        string title,
        string deliveryAddress,
        string description,
        string restockResponsible,
        string expectedErrorPart)
        {
            // Arrange
            InitialStepsForRestockItem();

            var selectItemPO = new SelectItemsForRestock_P0(_driver, _output);
            var createRestockPO = new CreateRestock_PO(_driver, _output);

            selectItemPO.SearchItems(itemName1, itemQuantityRestock1);
            selectItemPO.AddQuantityToItem(itemName1, itemQuantityRestock1);
            selectItemPO.ClickRestockButton();

            // Act
            createRestockPO.FillForm(
                title,
                deliveryAddress,
                description,
                restockResponsible
            );

            createRestockPO.Submit();
            createRestockPO.confirmRestockOrder();


            // Assert
            string actualError = createRestockPO.GetErrorText();
            Assert.Contains(expectedErrorPart, actualError);
        }

        [Theory]
        [Trait("LevelTesting", "Functional Testing")]
        [InlineData("", "Warehouse 1", "The Title field is required.")]
        [InlineData("Restock Order 1", "", "The DeliveryAddress field is required.")]
        public void UC7_Scen5_Validation_ConfirmErrors(
        string title,
        string deliveryAddress,
        string expectedError)
        {

            InitialStepsForRestockItem();

            var selectItemPO = new SelectItemsForRestock_P0(_driver, _output);
            var createRestockPO = new CreateRestock_PO(_driver, _output);

            selectItemPO.SearchItems(itemName1, itemQuantityRestock1);
            selectItemPO.AddQuantityToItem(itemName1, itemQuantityRestock1);
            selectItemPO.ClickRestockButton();

            createRestockPO.FillForm(
                title,
                deliveryAddress,
                "Restock for testing",
                ""
            );

            createRestockPO.Submit();


            string error = createRestockPO.GetValidationErrors();
            Assert.Contains(expectedError, error);
        }


        [Fact]
        [Trait("LevelTesting", "Functional Testing")]

        public void UC7_Scen6_Sprint3Exam()
        {

            var selectItemPO = new SelectItemsForRestock_P0(_driver, _output);
            var createRestockOrderPO = new CreateRestock_PO(_driver, _output);


            InitialStepsForRestockItem();
            selectItemPO.AddQuantityToItem(itemName1, itemQuantityRestock1);

            selectItemPO.SearchItems(itemName2, itemQuantityRestock2);
            selectItemPO.AddQuantityToItem(itemName2, itemQuantityRestock2);

            selectItemPO.RemoveItemInCart(itemId1);

            selectItemPO.ClickRestockButton();

            createRestockOrderPO.FillForm("Restock Order 11", "Muy lejos", "Restock for testing1 ", "test@gmail.com");

            Assert.True(createRestockOrderPO.TableHasItem(itemName2));

        }












    }
}
