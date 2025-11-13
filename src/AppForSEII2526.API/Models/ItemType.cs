namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class ItemType
    {
        public ItemType()
        {
            Items = new List<Item>();
        }
     

       


        public int Id { get; set; }
        public string? Name { get; set; }

        public IList<Item> Items { get; set; }




    }
}
