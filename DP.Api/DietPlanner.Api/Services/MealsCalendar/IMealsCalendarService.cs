using DietPlanner.Api.Models.MealsCalendar;
using System;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealsCalendar
{
    public interface IMealsCalendarService
    {
        Task<DailyMeals> GetDailyMeals(DateTime date);
    }
}
