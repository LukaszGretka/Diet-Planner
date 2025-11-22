using DietPlanner.Domain.Enums;
using System;

namespace DietPlanner.Api.Requests
{
    public class UpdateMealItemPortionRequest
    {
        public int ItemProductId { get; set; }

        // Optional to determine changed customized portion for product in dish 
        public int? DishProductId { get; set; }

        public ItemType ItemType { get; set; }

        public decimal CustomizedPortionMultiplier { get; set; }

        public DateTime Date { get; set; }
    }
}