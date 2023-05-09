using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Api.Models.MealsCalendar.DbModel
{
    public class MealProduct
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public UserMeal Meal { get; set; }
    }
}
