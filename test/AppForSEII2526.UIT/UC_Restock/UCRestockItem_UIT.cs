using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_Purchase;
using AppForSEII2526.UIT.UC_Restock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppForSEII2526.UIT.UC_Restock
{
    public class UCRestockItem_UIT : UC_UIT
    {
        private SelectItemsForRestock_P0 selectItemsForRestock_P0;
        private const string itemName1 = "Foam Roller";
        private const string itemQuantity1 = "18";

        private const string itemName2 = "Foam Roller";
        private const string itemQuantity2 = "0";

        private const string itemName3 = null;
        private const string itemQuantit3 = null;

        public UCRestockItem_UIT(ITestOutputHelper output) : base(output)
        {
            selectItemsForRestock_P0 = new SelectItemsForRestock_P0(_driver, _output);

        }


        private void Precondition_performance_login()
        {
            Perform_login("test@gmail.com", "Password123!");
        }

        private void InitialStepsForRestockItem()
        {
            Precondition_performance_login();
            selectItemsForRestock_P0.WaitForBeingVisible(By.Id("SelectRestock"));
            _driver.FindElement(By.Id("SelectRestock")).Click();
        }

        [Fact]
        [Trait("Level Testing", "Functional Testing")]
        public void UC7_Scen_1_1_1_RestockItem_SuccessfulRestock()
        {
            var selectItemPO = new SelectItemsForRestock_P0(_driver, _output);
            var createRestockOrderPO = new CreateRestock_PO(_driver, _output);
            var DetailRestockOrderPO = new DetailRestock_PO(_driver, _output);


            InitialStepsForRestockItem();
            selectItemsForRestock_P0.SearchItems(itemName1, itemQuantity1);
            selectItemsForRestock_P0.AddQuantityToItem(itemName1, itemQuantity1);
            selectItemsForRestock_P0.ClickRestockButton();
            // Verification steps can be added here



        }

    }
}
