using DietPlanner.Domain.Repositories;
using DietPlanner.Domain.Entities;

namespace DietPlanner.Application.Interfaces.Repositories;

public interface ICustomizedMealDishRepository : IRepository<CustomizedMealDish>
{
    Task<CustomizedMealDish?> GetByMealDishIdAndDishProductIdAsync(int mealDishId, int dishProductId, CancellationToken ct);
}
