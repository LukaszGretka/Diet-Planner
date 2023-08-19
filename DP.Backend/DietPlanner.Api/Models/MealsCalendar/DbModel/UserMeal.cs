using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Api.Models.MealsCalendar.DbModel
{
    public class UserMeal
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Date { get; set; }

        public MealType MealType { get; set; }

        public int MealTypeId { get; set; }
    }
}