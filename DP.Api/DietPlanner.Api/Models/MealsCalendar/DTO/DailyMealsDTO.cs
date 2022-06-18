using DietPlanner.Api.Models.MealsCalendar;
using System;

namespace DietPlanner.Api.Models.Dto.MealsCalendar
{
    public class DailyMealsDTO
    {
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public Meal Breakfast { get; set; }
        public Meal Lunch { get; set; }
        public Meal Dinner { get; set; }
        public Meal Supper { get; set; }

    }
}
