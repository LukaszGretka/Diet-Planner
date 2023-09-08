using DietPlanner.Api.Models.MealsCalendar.DTO;
using System.Collections.Generic;

namespace DietPlanner.Api.Database.Models
{
    public class Meal
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public MealTypeEnum MealType { get; set; }

        public ICollection<Dish> Dishes { get; } = new List<Dish>();

        public string UserId { get; set; }
    }
}