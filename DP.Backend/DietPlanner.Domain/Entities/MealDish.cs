using DietPlanner.Domain.Entities.Base;

namespace DietPlanner.Domain.Entities
{
    public class MealDish : BaseEntity
    {
        public int DishId { get; set; }

        public Dish Dish { get; set; } = null!;

        public int MealId { get; set; }

        public Meal Meal { get; set; } = null!;
    }
}
