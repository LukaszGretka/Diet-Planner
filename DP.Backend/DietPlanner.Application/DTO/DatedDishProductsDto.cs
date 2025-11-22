using DietPlanner.Application.DTO.Dishes;
using System;
using System.Collections.Generic;

namespace DietPlanner.Api.Models.Dashboard
{
    public class DatedDishProductsDto
    {
        public DateTime Date { get; set; }

        public IEnumerable<DishProductsDTO> DishProducts { get; set; }
    }
}
