using DietPlanner.Api.Models.MealsCalendar.DbModel;
using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Api.Database.Models
{
    public class DishProducts
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        /// <summary>
        /// Default portion multiplayer which is set on dish create.
        /// </summary>
        public decimal PortionMultiplier { get; set; }

        public int DishId { get; set; }

        public Dish Dish { get; set; }
    }
}
