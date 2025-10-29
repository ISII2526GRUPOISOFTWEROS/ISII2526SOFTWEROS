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
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, provide a name for your planning")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Planning name must have at least 3 characters")]
            [Display(Name = "Planning Name")]
            public string PlanningName { get; set; }

            [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
            public string? Description { get; set; }

            [Required(ErrorMessage = "Please, specify how many weeks you plan to attend")]
            [Range(1, 52, ErrorMessage = "Number of weeks must be between 1 and 52")]
            [Display(Name = "Number of Weeks")]
            public int NumberOfWeeks { get; set; }

            [Display(Name = "Health Issues (optional)")]
            public string? HealthIssues { get; set; }

            [Display(Name = "Goals (optional)")]
            public string? Goals { get; set; }

            [Required(ErrorMessage = "Please, select at least one payment method")]
            [Display(Name = "Payment Method")]
            public PaymentMethod PaymentMethod { get; set; }

            [Required(ErrorMessage = "Please, select at least one class to plan")]
            public IList<ClassForPlanDTO> SelectedClasses { get; set; }

            [Display(Name = "Total Price")]
            [JsonPropertyName("TotalPrice")]
            public double TotalPrice
        {
                get
            {
                    return SelectedClasses.Sum(c => c.PricePerClass * NumberOfWeeks);
                }
            }
        }

        public class ClassForPlanDTO
        {
            [Required]
            public string ClassName { get; set; }

            [Required]
            public string ClassType { get; set; }

            [Required]
            public double PricePerClass { get; set; }

            [Required]
            public string Day { get; set; }

            [Required]
            public string Time { get; set; }
        }
    } 