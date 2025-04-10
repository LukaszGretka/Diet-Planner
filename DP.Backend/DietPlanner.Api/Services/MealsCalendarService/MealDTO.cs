using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.Enums;
using DietPlanner.Api.Models.MealsCalendar.DTO;
using System.Collections.Generic;

namespace DietPlanner.Api.Services.MealsCalendar
{
    internal class MealDTO
    {
        public MealType MealType { get; set; }
        public List<DishDTO> Dishes { get; set; }
    }
}