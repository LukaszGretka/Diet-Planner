using DietPlanner.Api.Models.MealsCalendar.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Api.Database.Models
{
    public class Meal
    {
        [Key]
        public int Id { get; set; }

        public string Date { get; set; }

        public MealTypeEnum MealType { get; set; }

        public ICollection<MealDish> MealDishes { get; } = new List<MealDish>();

        public string UserId { get; set; }
    }
}