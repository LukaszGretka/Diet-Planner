using DietPlanner.Api.Database.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace DietPlanner.Api.Models.MealsCalendar.DbModel
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public float? Carbohydrates { get; set; }

        public float? Proteins { get; set; }

        public float? Fats { get; set; }

        public float? Calories { get; set; }

        public long? BarCode { get; set; }
    }
}
