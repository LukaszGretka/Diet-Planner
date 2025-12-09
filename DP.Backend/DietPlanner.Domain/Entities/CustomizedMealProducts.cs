using DietPlanner.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Domain.Entities
{
    public class CustomizedMealProducts: BaseEntity
    {
        [ForeignKey(nameof(MealProduct))]
        public int MealProductId { get; set; }

        public MealProduct MealProduct { get; set; } = null!;

        [Precision(6, 2)]
        public decimal? CustomizedPortionMultiplier { get; set; }
    }
}
