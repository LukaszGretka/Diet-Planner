using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Api.Database.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public ICollection<DishProducts> DishProducts { get; set; }

        public int MealId { get; set; }

        public Meal Meal { get; set; } = null!;

        public string UserId { get; set; }

        public bool ExposedForOtherUsers { get; set; }
    }
}
