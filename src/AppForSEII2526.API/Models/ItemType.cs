namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class ItemType
    {
<<<<<<< HEAD
        private string v;

        public ItemType(string v)
        {
            this.v = v;
        }

=======
        public ItemType()
        {
        }

        public ItemType(int id, string? name, IList<Item> items)
        {
            Id = id;
            Name = name;
            Items = items;
        }



>>>>>>> development
        public int Id { get; set; }
        public string? Name { get; set; }

        //Reference
        public IList<Item> Items { get; set; }

    }
}
