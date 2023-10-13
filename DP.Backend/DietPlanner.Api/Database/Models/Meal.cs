
using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Api.Database.Models
{
    public class Meal
    {
        [Key]
        public int Id { get; set; }

        public string Date { get; set; }

        public int MealType { get; set; }

        public string UserId { get; set; }
    }
}