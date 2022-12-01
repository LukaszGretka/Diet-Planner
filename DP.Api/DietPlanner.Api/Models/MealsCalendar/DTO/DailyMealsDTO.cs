using DietPlanner.Api.Models.MealsCalendar;
using System;

namespace DietPlanner.Api.Models.Dto.MealsCalendar
{
    public class DailyMealsDTO
    {
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public MealDTO Breakfast { get; set; }
        public MealDTO Lunch { get; set; }
        public MealDTO Dinner { get; set; }
        public MealDTO Supper { get; set; }

    }
}
