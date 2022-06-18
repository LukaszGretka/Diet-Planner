using DietPlanner.Api.Models;
using DietPlanner.Api.Models.Dto.MealsCalendar;
using DietPlanner.Api.Models.MealsCalendar;
using System;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealsCalendar
{
    public interface IMealsCalendarService
    {
        Task<DailyMealsDTO> GetDailyMeals(DateTime date);

        Task<DatabaseActionResult<Meal>> AddMeal(DateTime date, Meal meal);
    }
}
