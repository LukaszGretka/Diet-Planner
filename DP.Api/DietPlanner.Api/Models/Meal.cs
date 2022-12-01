using DietPlanner.Api.Models.MealsCalendar;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Api.Models
{
    public class Meal
    {
        public int Id { get; set; }

        public string Date { get; set; }

        [ForeignKey("MealType")]
        public MealTypeEnum MealTypeId { get; set; }

        public virtual MealType MealType { get; set; }
    }
}