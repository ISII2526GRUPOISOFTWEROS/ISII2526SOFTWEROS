namespace AppForSEII2526.API.Models
{
    public class Incident
    {
        public DateTime DateOdIdentification{ get; set; }
        public string? Description { get; set; }
        public string Exercise { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        
        //Reference
        public ApplicationUser AplicationUsers { get; set; }
        public IncidentState IncidentState { get; set; }
        
        public IList<IncidentItem> IncidentItems { get; set; }
    }
}
