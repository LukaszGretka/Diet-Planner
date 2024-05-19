using DietPlanner.Api.Enums;
using DietPlanner.Shared.Models;
using System;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealProductService
{
    public interface IMealProductService
    {
        public Task<DatabaseActionResult> AddOrUpdateCustomizedPortionMultiplier(int dishId,  int productId, int mealDishId, decimal multiplier);
    }
}