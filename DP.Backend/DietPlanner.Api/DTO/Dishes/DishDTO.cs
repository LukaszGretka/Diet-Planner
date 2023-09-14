using System.Collections.Generic;

namespace DietPlanner.Api.DTO.Dishes
{
    public class DishDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public bool ExposeToOtherUsers { get; set; }
    }
}
