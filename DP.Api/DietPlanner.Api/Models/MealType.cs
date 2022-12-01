using DietPlanner.Api.Models.MealsCalendar;
using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Api.Models
{
    public class MealType
    {
        [Key]
        public MealTypeEnum Id { get; set; }

        public string Name { get; set; }
    }
}
