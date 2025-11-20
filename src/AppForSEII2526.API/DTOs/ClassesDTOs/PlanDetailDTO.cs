using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.Models;
using Humanizer;

namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class PlanDetailDTO
    {
        public PlanDetailDTO(int id, string userName, DateTime dateCreated, decimal totalPrice, string planName, string description, int numberOfWeeks, string healthIssues, IList<ClassForPlanDTO> classes)
        {
            Id = id;
            UserName = userName;
            DateCreated = dateCreated;
            TotalPrice = totalPrice;
            PlanName = planName;
            Description = description;
            NumberOfWeeks = numberOfWeeks;
            HealthIssues = healthIssues;
            Classes = classes;
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal TotalPrice { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public int NumberOfWeeks { get; set; }
        public IList<ClassForPlanDTO> Classes { get; set; }
        public string HealthIssues { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PlanDetailDTO dTO &&
                   Id == dTO.Id &&
                   UserName == dTO.UserName &&
                   DateCreated == dTO.DateCreated &&
                   TotalPrice == dTO.TotalPrice &&
                   PlanName == dTO.PlanName &&
                   Description == dTO.Description &&
                   NumberOfWeeks == dTO.NumberOfWeeks &&
                   EqualityComparer<IList<ClassForPlanDTO>>.Default.Equals(Classes, dTO.Classes) &&
                   HealthIssues == dTO.HealthIssues;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(UserName);
            hash.Add(DateCreated);
            hash.Add(TotalPrice);
            hash.Add(PlanName);
            hash.Add(Description);
            hash.Add(NumberOfWeeks);
            hash.Add(Classes);
            hash.Add(HealthIssues);
            return hash.ToHashCode();
        }
    }
}