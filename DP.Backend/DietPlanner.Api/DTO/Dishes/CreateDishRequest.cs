using DietPlanner.Api.Database.Models;
using System.Collections.Generic;

namespace DietPlanner.Api.DTO.Dishes
{
    public class CreateDishRequest
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public ICollection<DishProducts> Products { get; set;  }
    }
}
