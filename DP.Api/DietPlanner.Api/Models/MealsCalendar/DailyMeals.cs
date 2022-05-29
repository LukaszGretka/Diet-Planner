namespace DietPlanner.Api.Models.MealsCalendar
{
    public class DailyMeals
    {
        public SpecifiedMeal Breakfast { get; set; }
        public SpecifiedMeal Lunch { get; set; }
        public SpecifiedMeal Dinner { get; set; }
        public SpecifiedMeal Supper { get; set; }

    }
}
