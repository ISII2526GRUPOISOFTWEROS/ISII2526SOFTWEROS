using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.ClassesDTOs
{


    public class PlanForCreateDTO{
       public PlanForCreateDTO(string Name, string? description, int weeks, string? healthIssues,IList<ClassForPlanDTO> selectedClasses,PaymentMethod paymentMethod){
            Name = Name;
            Description = description;
            Weeks = weeks;
            HealthIssues = healthIssues;
            SelectedClasses = selectedClasses;
            PaymentMethod = paymentMethod;
        }
        [Required]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Plan name must have at least 3 characters")]
        public string Name { get; set; }
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        public string? Description { get; set; }
        [Required]
        [Range(1, 52, ErrorMessage = "Weeks must be between 1 and 52")]
        public int Weeks { get; set; }
        [StringLength(100, ErrorMessage = "Health issues cannot exceed 100 characters")]
        public string? HealthIssues { get; set; }
        [Required]
        public IList<ClassForPlanDTO> SelectedClasses { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        public override bool Equals(object? obj)
        {
            return obj is PlanForCreateDTO dto &&
                   Name == dto.Name &&
                   Description == dto.Description &&
                   Weeks == dto.Weeks &&
                   HealthIssues == dto.HealthIssues &&
                   SelectedClasses.SequenceEqual(dto.SelectedClasses) &&
                   PaymentMethod == dto.PaymentMethod;
        }
    }
}