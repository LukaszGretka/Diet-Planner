using DietPlanner.Domain.Entities.Products;

namespace DietPlanner.Domain.Entities.Meals
{
    public class MealProduct
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int MealId { get; set; }

        public Meal Meal { get; set; } = null!;
    }
}
