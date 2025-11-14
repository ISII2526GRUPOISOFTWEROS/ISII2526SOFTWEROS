using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.ClassesDTOs
{
    public class PlanForCreateDTO
    {
        public PlanForCreateDTO() { }
        public PlanForCreateDTO(string Name, string userName, string? description, int weeks, string? healthIssues, IList<ClassForPlanDTO> selectedClasses, int paymentMethodId)
        {
            this.Name = Name;
            this.UserName = userName;
            this.Description = description;
            this.Weeks = weeks;
            this.HealthIssues = healthIssues;
            this.PaymentMethodId = paymentMethodId;
        }
        [Required]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Plan name must have at least 3 characters")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Plan name must have at least 10 characters")]
        public string UserName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        public string? Description { get; set; }

        [Required]
        [Range(1, 52, ErrorMessage = "Weeks must be between 1 and 52")]
        public int Weeks { get; set; }

        [StringLength(100, ErrorMessage = "Health issues cannot exceed 100 characters")]
        public string? HealthIssues { get; set; }

        [Required]
        public IList<ClassSelectionDTO> SelectedClasses { get; set; } = new List<ClassSelectionDTO>();

        [Required]
        public int PaymentMethodId { get; set; }  
     
        public IList<GoalForClassDTO>? Goals { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PlanForCreateDTO dTO &&
                   Name == dTO.Name &&
                   UserName == dTO.UserName &&
                   Description == dTO.Description &&
                   Weeks == dTO.Weeks &&
                   HealthIssues == dTO.HealthIssues &&
                   EqualityComparer<IList<ClassSelectionDTO>>.Default.Equals(SelectedClasses, dTO.SelectedClasses) &&
                   PaymentMethodId == dTO.PaymentMethodId &&
                   EqualityComparer<IList<GoalForClassDTO>?>.Default.Equals(Goals, dTO.Goals);
        }
    }

    public class ClassSelectionDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public IList<string>? ItemType { get; set; }
    }

    public class GoalForClassDTO
    {
        [Required]
        public int ClassId { get; set; }

        public string? Goal { get; set; }
    }
}
