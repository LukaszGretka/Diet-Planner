namespace DietPlanner.Domain.Entities
{
    public class Meal : BaseEntity
    {
        public DateTime Date { get; set; }

        public int MealType { get; set; }

        public string? UserId { get; set; }
    }
}