using AppForSEII2526.Web.API;
using System;
namespace AppForSEII2526.Web
{
    public class ItemPStateContainer
    {
        public ItemForPurchaseDTO? SelectedItem { get; private set; }

        public event Action? OnChange;

        public void SetSelectedItem(ItemForPurchaseDTO item)
        {
            SelectedItem = item;
            OnChange?.Invoke();
        }

        public void ClearSelectedItem()
        {
            SelectedItem = null;
            OnChange?.Invoke();
        }

    }
}
