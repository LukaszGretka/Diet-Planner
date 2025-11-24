using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Domain.Entities
{
    public class CustomizedMealProducts
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(MealProduct))]
        public int MealProductId { get; set; }

        public MealProduct MealProduct { get; set; } = null!;

        [Precision(6, 2)]
        public decimal CustomizedPortionMultiplier { get; set; }
    }
}
