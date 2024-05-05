using DietPlanner.Api.Models.MealsCalendar.DbModel;

namespace DietPlanner.Api.Database.Models
{
    public class CustomizedDishProducts
    {
        public int Id { get; set; }

        public int DishProductId { get; set; }

        public DishProducts DishProduct { get; set; } = null!;

        public int MealDishId { get; set; }

        public MealDish MealDish { get; set; } = null!;

        public decimal CustomizedPortionMultiplier { get; set; }
    }
}
