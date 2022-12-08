using DietPlanner.Api.Models.MealsCalendar;
using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Api.Models
{
    public class MealType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Meal Meal { get; set; }
    }
}
