using System;

namespace DietPlanner.Api.Models.MealsCalendar
{
    public class Meal
    {
        public MealTypeEnum MealType { get; set; }

        public Product[] Products { get; set; }
    }

    public class MealByDay : Meal
    {
        public DateTime Date { get; set; }
    }

    public enum MealTypeEnum
    {
        Breakfast = 0,
        Lunch = 1,
        Dinner = 2,
        Supper = 3,
    }
}
