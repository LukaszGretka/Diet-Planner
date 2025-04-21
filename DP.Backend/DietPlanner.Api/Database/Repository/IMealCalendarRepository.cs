using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.DTO.Products;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DietPlanner.Api.Database.Repository
{
    public interface IMealCalendarRepository
    {
        public Task<List<DishDTO>> GetMealDishes(Meal meal, CancellationToken ct);

        public Task<List<ProductDTO>> GetMealProducts(Meal meal, CancellationToken ct);
    }
}
