﻿using System.ComponentModel.DataAnnotations;

namespace DietPlanner.Api.Models.MealsCalendar.DbModel
{
    public class UserPortionProduct : UserProduct
    {
        public decimal PortionMultiplier { get; set; }
    }

    public class UserProduct : Product
    {
        // User ID from Identity database (AspNetUser table) 
        public string UserId { get; set; }

        // Determines if other users can use this product for their meals
        public bool ExposedForOtherUsers { get; set; }
    }

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
