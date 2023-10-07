using DietPlanner.Api.Enums;
using System;

namespace DietPlanner.Api.Models.MealProductModel
{
    public class UpdateMultiplierRequest
    {
        public int ProductId { get; set; }

        public int DishId { get; set; }

        public decimal PortionMultiplier { get; set; }
    }
}