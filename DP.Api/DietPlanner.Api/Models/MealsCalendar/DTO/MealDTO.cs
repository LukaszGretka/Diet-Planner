using System;

namespace DietPlanner.Api.Models.MealsCalendar
{
    public class MealDTO
    {
        public MealTypeEnum MealType { get; set; }

        public Product[] Products { get; set; }
    }

    public class MealByDay : MealDTO
    {
        public DateTime Date { get; set; }
    }

    public enum MealTypeEnum
    {
        Breakfast = 1,
        Lunch = 2,
        Dinner = 3,
        Supper = 4,
    }
}
