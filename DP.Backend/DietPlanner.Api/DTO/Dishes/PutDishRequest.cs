﻿using DietPlanner.Api.Database.Models;
using System.Collections.Generic;

namespace DietPlanner.Api.DTO.Dishes
{
    public class PutDishRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public ICollection<DishProductsDTO> Products { get; set;  }

        public bool ExposeToOtherUsers { get; set; }
    }
}
