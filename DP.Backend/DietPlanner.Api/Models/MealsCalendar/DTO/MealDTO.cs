﻿using System;
using System.Collections.Generic;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;

namespace DietPlanner.Api.Models.MealsCalendar.DTO
{
    public class MealDto
    {
        public MealTypeEnum MealTypeId { get; set; }

        public List<DishDTO> Dishes { get; set; }
    }

    public class PutMealRequest : MealDto
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
