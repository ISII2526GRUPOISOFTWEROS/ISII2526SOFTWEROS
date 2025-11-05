using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class PlanDetailDTO : PlanForCreateDTO
    {
        public PlanDetailDTO( int id,string userFullName, DateTime dateCreated,decimal totalPrice,string planName,string description,int numberOfWeeks,string healthIssues,
            IList<ClassForPlanDTO> classes) : base(userFullName,dateCreated,totalPrice,planName,description,numberOfWeeks,healthIssues,classes)
        {Id = id;
        }

        public int Id { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PlanDetailDTO dto &&
                   base.Equals(obj) &&
                   UserFullName == dto.UserFullName &&
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
            return HashCode.Combine(UserFullName, DateCreated, TotalPrice, PlanName, Description, NumberOfWeeks, HealthIssues, Classes);
        }
    }
}