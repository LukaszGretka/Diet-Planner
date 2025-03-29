using System;
using System.Collections.Generic;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.DTO.Products;
using DietPlanner.Api.Enums;

namespace DietPlanner.Api.Models.MealsCalendar.DTO
{
    public class MealDto
    {
        public MealType MealType { get; set; }

        public List<DishDTO> Dishes { get; set; }

        public List<ProductDTO> Products { get; set; }
    }

    public class PutMealRequest : MealDto
    {
        public DateTime Date { get; set; }
    }

    public class MealProductDto
    {
        public DateTime Date { get; set; }

        public MealType MealType { get; set; }

        public ProductDTO Product { get; set; }
    }
}
