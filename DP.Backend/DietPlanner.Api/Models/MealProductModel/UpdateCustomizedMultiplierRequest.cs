using DietPlanner.Api.Enums;
using System;

namespace DietPlanner.Api.Models.MealProductModel
{
    public class UpdateCustomizedMultiplierRequest
    {
        public int ProductId { get; set; }

        public int DishId { get; set; }

        public decimal CustomizedPortionMultiplier { get; set; }
    }
}