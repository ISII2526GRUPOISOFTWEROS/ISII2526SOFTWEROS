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
            this.userName = userName;
            DateCreated = dateCreated;
            TotalPrice = totalPrice;
            PlanName = planName;
            Description = description;
            NumberOfWeeks = numberOfWeeks;
            HealthIssues = healthIssues;
            Classes = classes;
        }
        public int Id { get; set; }
        public string userName { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal TotalPrice { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public int NumberOfWeeks { get; set; }
        public IList<ClassForPlanDTO> Classes { get; set; }
        public string HealthIssues { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PlanDetailDTO dto &&
                   base.Equals(obj) &&
                   userName == dto.userName &&
                   DateCreated == dto.DateCreated &&
                   TotalPrice == dto.TotalPrice &&
                   PlanName == dto.PlanName &&
                   Description == dto.Description &&
                   NumberOfWeeks == dto.NumberOfWeeks &&
                   HealthIssues == dto.HealthIssues &&
                   EqualityComparer<IList<ClassForPlanDTO>>.Default.Equals(Classes, dto.Classes);
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(userName, DateCreated, TotalPrice, PlanName, Description, NumberOfWeeks, HealthIssues, Classes);
        }
    }
}