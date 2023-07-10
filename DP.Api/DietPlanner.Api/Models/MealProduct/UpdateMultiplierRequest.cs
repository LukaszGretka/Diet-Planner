using DietPlanner.Api.Enums;
using System;

namespace DietPlanner.Api.Models.MealProduct
{
    public class UpdateMultiplierRequest
    {
        public int ProductId { get; set; }
        
        public MealType MealType { get; set; }
        
        public DateTime Date { get; set; }

        public decimal PortionMultiplier { get; set; }
    }
}
