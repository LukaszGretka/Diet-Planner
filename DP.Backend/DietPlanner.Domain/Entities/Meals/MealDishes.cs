using DietPlanner.Domain.Entities.Dishes;

namespace DietPlanner.Domain.Entities.Meals
{
    public class MealDish
    {
        public int Id { get; set; }

        public int DishId { get; set; }

        public Dish Dish { get; set; } = null!;

        public int MealId { get; set; }

        public Meal Meal { get; set; } = null!;
    }
}
