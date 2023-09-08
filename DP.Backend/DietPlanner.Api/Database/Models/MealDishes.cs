using DietPlanner.Api.Models.MealsCalendar.DbModel;

namespace DietPlanner.Api.Database.Models
{
    public class MealDishes
    {
        public int Id { get; set; }

        public Dish Dishes { get; set; }

        public Meal Meals { get; set; }
    }
}
