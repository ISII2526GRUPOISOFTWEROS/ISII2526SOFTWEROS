using AppForSEII2526.API.DTOs.ClassesDTOs;
using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PlanStateContainer
    {
        public AppForSEII2526.API.DTOs.ClassesDTOs.PlanForCreateDTO Plan { get; private set; } = new AppForSEII2526.API.DTOs.ClassesDTOs.PlanForCreateDTO()
        {
            SelectedClasses = new List<AppForSEII2526.API.DTOs.ClassesDTOs.ClassSelectionDTO>()
        };

        public decimal TotalPrice
        {
            get
            {
                return Plan.SelectedClasses?.Sum(pi => pi.Price) ?? 0m;
            }
        }

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddClassToPlan(AppForSEII2526.Web.API.ClassForPlanDTO classDto)
        {
            if (Plan.SelectedClasses == null) Plan.SelectedClasses = new List<AppForSEII2526.API.DTOs.ClassesDTOs.ClassSelectionDTO>();

            if (!Plan.SelectedClasses.Any(pi => pi.Id == classDto.Id))
            {
                var selectionDto = new AppForSEII2526.API.DTOs.ClassesDTOs.ClassSelectionDTO
                {
                    Id = classDto.Id,
                    Name = classDto.Name ?? "Unknown Class",
                    Price = (decimal)classDto.Price,
                    Date = classDto.Date.DateTime,
                    ItemType = classDto.ItemType?.ToList() ?? new List<string>()
                };

                Plan.SelectedClasses.Add(selectionDto);
                NotifyStateChanged();
            }
        }

       
        public void AddClassToPlan(AppForSEII2526.API.DTOs.ClassesDTOs.ClassSelectionDTO classDto)
        {
            if (Plan.SelectedClasses == null) Plan.SelectedClasses = new List<AppForSEII2526.API.DTOs.ClassesDTOs.ClassSelectionDTO>();

            if (!Plan.SelectedClasses.Any(pi => pi.Id == classDto.Id))
            {
                Plan.SelectedClasses.Add(classDto);
                NotifyStateChanged();
            }
        }


        public void RemovePlanItem(AppForSEII2526.API.DTOs.ClassesDTOs.ClassSelectionDTO item)
        {
            Plan.SelectedClasses?.Remove(item);
            NotifyStateChanged();
        }

        public void ClearPlanningCart()
        {
            Plan.SelectedClasses?.Clear();
            NotifyStateChanged();
        }

        public void PlanProcessed()
        {
            Plan = new AppForSEII2526.API.DTOs.ClassesDTOs.PlanForCreateDTO()
            {
                SelectedClasses = new List<AppForSEII2526.API.DTOs.ClassesDTOs.ClassSelectionDTO>()
            };
            NotifyStateChanged();
        }
    }
}
