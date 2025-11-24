using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Domain.Entities
{
    public class Meal
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int MealType { get; set; }

        public string? UserId { get; set; }
    }
}