namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class ItemType
    {
        public ItemType()
        {
        }
        public ItemType(int id, string? name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string? Name { get; set; }

        //Reference
        public IList<Item> Items { get; set; }

    }
}
