using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Application.DTO.Products;
using DietPlanner.Domain.Entities.Meals;

namespace DietPlanner.Application.Interfaces
{
    public interface IMealCalendarRepository
    {
        public Task<List<DishDTO>> GetMealDishes(Meal meal, CancellationToken ct);

        public Task<List<ProductDTO>> GetMealProducts(Meal meal, CancellationToken ct);
    }
}
