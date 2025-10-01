namespace AppForSEII2526.API.Models
{
    public class ItemForExercise
    {
        [Key]
        public string Location { get; set; }

        //Reference
        public IList<IncidentItem> IncidentItems { get; set; }
        public Item Item { get; set; }
    }
}
