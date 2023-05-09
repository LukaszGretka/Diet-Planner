namespace DietPlanner.Api.Models.MealsCalendar.DTO
{
    public class DailyMealsDto
    {
        public MealDto Breakfast { get; set; }

        public MealDto Lunch { get; set; }

        public MealDto Dinner { get; set; }

        public MealDto Supper { get; set; }
    }
}
