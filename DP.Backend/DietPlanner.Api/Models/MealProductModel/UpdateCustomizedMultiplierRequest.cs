using DietPlanner.Api.Enums;
using System;

namespace DietPlanner.Api.Models.MealProductModel
{
    public class UpdateMealItemPortionRequest
    {
        public int ProductId { get; set; }

        public int DishId { get; set; }

        public int MealDishId { get; set; }

        public decimal CustomizedPortionMultiplier { get; set; }
    }
}