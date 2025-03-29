﻿using DietPlanner.Api.DTO;
using DietPlanner.Api.Enums;
using System;

namespace DietPlanner.Api.Models.MealsCalendar.Requests
{
    public class AddMealItemRequest
    {
        public int ItemId { get; set; }

        public ItemType ItemType { get; set; }

        public MealType MealType { get; set; }

        public DateTime Date { get; set; }
    }
}
