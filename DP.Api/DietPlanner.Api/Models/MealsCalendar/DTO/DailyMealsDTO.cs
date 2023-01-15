using DietPlanner.Api.Models.MealsCalendar;
using System;
using System.Collections.Generic;

namespace DietPlanner.Api.Models.Dto.MealsCalendar
{
    public class DailyMealsDTO
    {

        public (MealTypeEnum, List<Product>) Breakfastee { get; set; }

        public MealDTO Breakfast { get; set; }
        public MealDTO Lunch { get; set; }
        public MealDTO Dinner { get; set; }
        public MealDTO Supper { get; set; }
    }
}
