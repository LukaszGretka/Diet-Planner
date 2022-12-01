using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Api.Models
{
    public class MealProduct
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [ForeignKey("Meals")]
        public int MealId { get; set; }

        public virtual Meal Meals { get; set; }
    }
}
