using DietPlanner.Api.Models.MealsCalendar;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Api.Models
{
    public class Meal
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public MealType MealType { get; set; }

        public int MealTypeId { get; set; }
    }
}