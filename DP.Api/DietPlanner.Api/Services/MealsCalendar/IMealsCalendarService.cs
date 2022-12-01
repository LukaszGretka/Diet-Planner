using DietPlanner.Api.Models;
using DietPlanner.Api.Models.Dto.MealsCalendar;
using DietPlanner.Api.Models.MealsCalendar;
using DietPlanner.Shared.Models;
using System;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealsCalendar
{
    public interface IMealsCalendarService
    {
        Task<DailyMealsDTO> GetMeals(DateTime date);

        Task<DatabaseActionResult<Meal>> AddMeal(MealByDay mealByDay);
    }
}
