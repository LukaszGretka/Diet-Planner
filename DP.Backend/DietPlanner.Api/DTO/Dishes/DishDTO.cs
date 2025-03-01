using System.Collections.Generic;

namespace DietPlanner.Api.DTO.Dishes
{
    public class DishDTO
    {
        public int Id { get; set; }

        public int MealDishId { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public bool ExposeToOtherUsers { get; set; }

        // This property is used to determine if the dish was created by currenly signed in user
        public bool IsOwner { get; set; }

        public IEnumerable<DishProductsDTO> Products { get; set; }
    }
}
