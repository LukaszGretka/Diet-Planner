using DietPlanner.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietPlanner.Domain.Entities.Dishes
{
    public class DishProducts
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        /// <summary>
        /// Default portion multiplayer which is set on dish create.
        /// </summary>
        [Precision(3, 2)]
        public decimal PortionMultiplier { get; set; }

        public int DishId { get; set; }

        public Dish Dish { get; set; }
    }
}
