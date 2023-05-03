using DietPlanner.Api.Models;
using DietPlanner.Api.Models.MealsCalendar;
using DietPlanner.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealsCalendar
{
    public interface IMealsCalendarService
    {
        Task<List<MealDTO>> GetMeals(DateTime date, string userId);

        Task<DatabaseActionResult<UserMeal>> AddOrUpdateMeal(MealByDay mealByDay, string userId);
    }
}
