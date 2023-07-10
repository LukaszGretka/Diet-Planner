using DietPlanner.Api.Enums;
using DietPlanner.Shared.Models;
using System;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealProductService
{
    public interface IMealProductService
    {
        public Task<DatabaseActionResult> UpdatePortionMultiplier(DateTime date, MealType mealType, int productId, decimal multiplier);
    }
}
