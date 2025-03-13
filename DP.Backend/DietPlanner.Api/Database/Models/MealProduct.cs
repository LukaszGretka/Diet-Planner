using DietPlanner.Api.Models.MealsCalendar.DbModel;

namespace DietPlanner.Api.Database.Models
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
