namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(IncidentId), nameof(ItemId))]
    public class IncidentItem
    {
        public int IncidentId { get; set; }
        public int ItemId { get; set; }

        //Reference
        public Incident Incident { get; set; }
        public IncidentPriority IncidentPriority { get; set; }
        public ItemForExercise ItemForExercise { get; set; }
    }
}
