﻿namespace AppForSEII2526.API.Models
{
    public class Class
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public string Name { get; set; }
        [Precision(10, 2)]
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
      
        public IList<PlanItem> PlanItems { get; set; }

        public IList<TypeItem> TypeItems { get; set; } 

    }
}
