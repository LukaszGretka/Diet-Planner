using DietPlanner.Api.DTO;

namespace DietPlanner.Api.Models.MealProductModel
{
    public class UpdateMealItemPortionRequest
    {
        public int ItemProductId { get; set; }

        // Optional to determine changed customized portion for product in dish 
        public int? DishProductId { get; set; }

        public ItemType ItemType { get; set; }

        public decimal CustomizedPortionMultiplier { get; set; }
    }
}