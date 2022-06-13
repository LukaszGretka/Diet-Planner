using DietPlanner.Api.Models.MealsCalendar;
using System;

namespace DietPlanner.Api.Models.Dto.MealsCalendar
{
    public class DailyMealsDTO
    {
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public SpecifiedMeal Breakfast { get; set; }
        public SpecifiedMeal Lunch { get; set; }
        public SpecifiedMeal Dinner { get; set; }
        public SpecifiedMeal Supper { get; set; }

    }
}
