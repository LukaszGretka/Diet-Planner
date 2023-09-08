using DietPlanner.Api.Database.Models;
using DietPlanner.Api.Models.MealsCalendar.DTO;
using DietPlanner.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealProductService
{
    public interface IMealService
    {
        Task<List<MealDto>> GetMeals(DateTime date, string userId);

        Task<DatabaseActionResult<Meal>> AddOrUpdateMeal(MealByDay mealByDay, string userId);
    }
}
