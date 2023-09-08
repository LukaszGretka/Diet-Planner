using System;
using System.Collections.Generic;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.Models.MealsCalendar.DbModel;

namespace DietPlanner.Api.Models.MealsCalendar.DTO
{
    public class MealDto
    {
        public MealTypeEnum MealTypeId { get; set; }

        public List<DishProducts> DishProducts { get; set; }
    }

    public class MealByDay : MealDto
    {
        public DateTime Date { get; set; }
    }

    public class ProductPortion: Product
    {
        public decimal PortionMultiplier { get; set; }
    }

    public enum MealTypeEnum
    {
        Breakfast = 1,
        Lunch = 2,
        Dinner = 3,
        Supper = 4,
    }
}
