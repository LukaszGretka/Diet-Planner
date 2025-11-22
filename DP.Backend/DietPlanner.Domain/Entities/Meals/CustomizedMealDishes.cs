using DietPlanner.Domain.Entities.Dishes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Domain.Entities.Meals
{
    public class CustomizedMealDishes : BaseEntity
    {
        [ForeignKey(nameof(DishProduct))]
        public int DishProductId { get; set; }

        public DishProducts DishProduct { get; set; } = null!;

        [ForeignKey(nameof(MealDish))]
        public int MealDishId { get; set; }

        public MealDish MealDish { get; set; } = null!;

        [Precision(6, 2)]
        public decimal CustomizedPortionMultiplier { get; set; }
    }
}
