namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class ItemType
    {
        public ItemType()
        {
        }

        public ItemType(int id, string? name, IList<Item> items)
        {
            Id = id;
            Name = name;
            Items = items;
        }



        public int Id { get; set; }
        public string? Name { get; set; }

        public IList<Item> Items { get; set; }
        
    public ICollection<Class> Classes { get; set; } = new List<Class>();



    }
}
