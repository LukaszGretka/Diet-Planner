using DietPlanner.Api.Models.MealProductModel;
using DietPlanner.Api.Models.MealsCalendar.DTO;
using DietPlanner.Api.Models.MealsCalendar.Requests;
using DietPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealProductService
{
    public interface IMealService
    {
        Task<DatabaseActionResult<List<MealDto>>> GetMeals(DateTime date, string userId, CancellationToken ct);

        Task<DatabaseActionResult<List<MealDto>>> AddMealItem(MealItemRequest addMealItemRequest, string userId, CancellationToken ct);

        Task<DatabaseActionResult<List<MealDto>>> RemoveMealItem(MealItemRequest removeMealItemRequest, string userId, CancellationToken ct);

        Task<DatabaseActionResult<List<MealDto>>> UpdateMealItemPortion(UpdateMealItemPortionRequest request, string userId, CancellationToken ct);
    }
}
