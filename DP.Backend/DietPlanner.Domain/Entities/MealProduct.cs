namespace DietPlanner.Domain.Entities
{
    public class MealProduct : BaseEntity
    {
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int MealId { get; set; }

        public Meal Meal { get; set; } = null!;
    }
}
