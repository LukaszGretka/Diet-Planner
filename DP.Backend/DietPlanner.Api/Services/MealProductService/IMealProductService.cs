using DietPlanner.Api.Enums;
using DietPlanner.Shared.Models;
using System;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealProductService
{
    public interface IMealProductService
    {
        public Task<DatabaseActionResult> UpdatePortionMultiplier(int dishId,  int productId, decimal multiplier);
    }
}