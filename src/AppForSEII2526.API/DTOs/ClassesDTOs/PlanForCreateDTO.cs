namespace AppForSEII2526.API.DTOs.ClassesDTOs
{
    public class PlanForCreateDTO
    {
        public PlanForCreateDTO(string name, string? description, int weeks, string? healthIssues, string? goals, PaymentMethod paymentMethod, IList<ClassForPlanDTO> SelectedClasses)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));
            description = description;
            weeks = weeks;
            healthIssues = healthIssues;
            goals = goals;
            paymentMethod = paymentMethod;
            SelectedClasses = SelectedClasses ?? throw new ArgumentNullException(nameof(SelectedClasses));
        }
        public PlanForCreateDTO()
        {
            SelectedClasses = new List<ClassForPlanDTO>();
        }

    }
}
