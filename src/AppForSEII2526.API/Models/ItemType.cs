namespace AppForSEII2526.API.Models
{
    public class ItemType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Reference
        public IList<Item> Items { get; set; }

    }
}
