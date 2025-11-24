using AppForSEII2526.Web.API;

//namespace AppForSEII2526.Web
//{
//    public class PlanStateContainer
//    {

//        public PlanForCreateDTO Plan { get; private set; } = new PlanForCreateDTO()
//        {
//            PlanItems = new List<PlanItemDTO>()
//        };

//        public decimal TotalPrice
//        {
//            get
//            {
//                // Total depends on the price of each class multiplied by the number of classes
//                return Convert.ToDecimal(Plan.PlanItems.Sum(pi => pi.PriceForEnrolling));
//            }
//        }

//        public event Action? OnChange;
//        private void NotifyStateChanged() => OnChange?.Invoke();

//        // Add a class to the plan
//        public void AddClassToPlan(ClassForPlanDTO classDto)
//        {
//            // Check if the class is already added
//            if (!Plan.PlanItems.Any(pi => pi.ClassID == classDto.Id))
//            {
//                Plan.PlanItems.Add(new PlanItemDTO()
//                {
//                    ClassID = classDto.Id,
//                    Name = classDto.Name,
//                    Type = classDto.Type,
//                    PriceForEnrolling = classDto.PriceForEnrolling,
//                    Day = classDto.Day,
//                    Time = classDto.Time
//                });
//                NotifyStateChanged();
//            }
//        }

//        // Remove a class from the selected planning
//        public void RemovePlanItem(PlanItemDTO item)
//        {
//            Plan.PlanItems.Remove(item);
//            NotifyStateChanged();
//        }

//        // Clear all the selected classes
//        public void ClearPlanningCart()
//        {
//            Plan.PlanItems.Clear();
//            NotifyStateChanged();
//        }

//        // Once the plan has been completed, we create a new one
//        public void PlanProcessed()
//        {
//            Plan = new PlanForCreateDTO()
//            {
//                PlanItems = new List<PlanItemDTO>()
//            };
//            NotifyStateChanged();
//        }
//    }
//}
