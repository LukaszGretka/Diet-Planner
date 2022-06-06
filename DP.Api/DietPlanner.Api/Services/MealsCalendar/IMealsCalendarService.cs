using DietPlanner.Api.Models.Dto.MealsCalendar;
using System;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealsCalendar
{
    public interface IMealsCalendarService
    {
        Task<DailyMealsDTO> GetDailyMeals(DateTime date);
    }
}
