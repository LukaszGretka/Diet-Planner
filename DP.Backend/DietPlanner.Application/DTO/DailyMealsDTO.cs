namespace DietPlanner.Application.DTO
{
    public class DailyMealsDto
    {
        public required MealDto Breakfast { get; set; }

        public required MealDto Lunch { get; set; }

        public required MealDto Dinner { get; set; }

        public required MealDto Supper { get; set; }
    }
}
