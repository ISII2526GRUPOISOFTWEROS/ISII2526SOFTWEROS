using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class RestockStateContainer
    {
        // We create an instance of Restock when an instance of RestockStateContainer is created 
        public RestockForCreateDTO Restock { get; private set; } = new RestockForCreateDTO()
        {
            RestockItems = new List<RestockItemDTO>()
        };

        // We compute the TotalPrice of the movies we have selected for restocking
        // Assuming RestockItemDTO has a Quantity and PurchasePrice
        public Dictionary<int, string> ItemNames { get; private set; } = new Dictionary<int, string>();
        public decimal TotalPrice
        {
            get
            {
                return Convert.ToDecimal(Restock.RestockItems.Sum(ri => ri.RestockPrice * ri.Quantity));
            }
        }

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddItemToRestock(ItemForRestockDTO item)
        {
            // Check if the movie is already in the restock list
            var existingItem = Restock.RestockItems.FirstOrDefault(ri => ri.ItemId == item.Id);

            if (existingItem == null)
            {
                // We add it if it is not in the list 
                Restock.RestockItems.Add(new RestockItemDTO()
                {
                    ItemId = item.Id,
                    ItemName = item.Name, 
                    RestockPrice = item.RestockPrice,
                    Quantity = 1 // Default quantity
                });
            }
            else
            {
                // If it is already in the list, we increment the quantity
                existingItem.Quantity++;
            }

            NotifyStateChanged();
        }

        // To delete movies from the list of selected movies 
        public void RemoveRestockItem(RestockItemDTO item)
        {
            Restock.RestockItems.Remove(item);
            NotifyStateChanged();
        }

        // We eliminate all the movies from the list 
        public void ClearRestockCart()
        {
            Restock.RestockItems.Clear();
            NotifyStateChanged();
        }

        // We have already finished the process of restocking, thus, we create a new Restock 
        public void RestockProcessed()
        {
            // We have finished the restock process so we create a new object without data 
            Restock = new RestockForCreateDTO()
            {
                RestockItems = new List<RestockItemDTO>()
            };
            NotifyStateChanged();
        }
    }
}
