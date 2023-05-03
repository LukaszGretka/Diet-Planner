namespace DietPlanner.Api.Models
{
    public class MealType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public UserMeal Meal { get; set; }
    }
}
