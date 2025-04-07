using DietPlanner.Api.Enums;
using System;

namespace DietPlanner.Api.Models.MealProductModel
{
    public class UpdateMealItemPortionRequest
    {
        public int ItemProductId { get; set; }

        public int? DishProductId { get; set; } // For update dish products

        public decimal CustomizedPortionMultiplier { get; set; }
    }
}