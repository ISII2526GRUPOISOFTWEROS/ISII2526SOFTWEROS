using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {
        public ItemForCreateDTO Purchase { get; private set; } = new ItemForCreateDTO()
        {
            PurchaseItems = new List<CreatePurchaseItemDTO>()
        };
        public Dictionary<int, string> ItemNames { get; private set; } = new Dictionary<int, string>();
        public decimal TotalPrice
        {
            get
            {
                 return Convert.ToDecimal(Purchase.PurchaseItems.Sum(i=> i.Price * i.Quantity));
            }

        }
        private event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
        public void AddPurchaseItem(ItemForPurchaseDTO item)
        {
            var existing = Purchase.PurchaseItems.FirstOrDefault(i => i.ItemId == item.Id);
            if (!ItemNames.ContainsKey(item.Id)){
                ItemNames[item.Id] = item.Name;
            }

            if (existing is null)
            {
                Purchase.PurchaseItems.Add(new CreatePurchaseItemDTO
                {
                    ItemId = item.Id,
                    Quantity = 1,
                    Price = item.Price

                });
            }
            else
            {
                existing.Quantity++;
            }
            NotifyStateChanged();

        }
        public void RemovePurchaseItem(int itemId)
        {
            var itemToRemove = Purchase.PurchaseItems.FirstOrDefault(i => i.ItemId == itemId);
            if (itemToRemove != null)
            {
                Purchase.PurchaseItems.Remove(itemToRemove);
                if(ItemNames.ContainsKey(itemId))
                {
                    ItemNames.Remove(itemId);
                }
                NotifyStateChanged();
            }
        }
        public void ClearPurchase()
        {
            Purchase.PurchaseItems.Clear();
            ItemNames.Clear();
            NotifyStateChanged();
        }
        public void PurchaseProcessed()
        {
            Purchase = new ItemForCreateDTO()
            {
                PurchaseItems = new List<CreatePurchaseItemDTO>()
            };
            ItemNames.Clear();
            NotifyStateChanged();
        }
    }
}
