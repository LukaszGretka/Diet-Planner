using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Api.Models
{
    public class MealProduct
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public Meal Meal { get; set; }
    }
}
