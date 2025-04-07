using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.DTO.Products;
using System.Collections.Generic;

namespace DietPlanner.Api.Database.Repository
{
    public interface IMealCalendarRepository
    {
        public List<DishDTO> GetMealDishes(Meal meal);

        public List<ProductDTO> GetMealProducts(Meal meal);
    }
}
