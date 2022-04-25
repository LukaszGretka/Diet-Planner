﻿namespace DietPlanner.Api.Models
{
    public class Product
    {
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
