using DietPlanner.Domain.Repositories;
using DietPlanner.Domain.Entities;

namespace DietPlanner.Application.Interfaces.Repositories;

public interface IMealDishRepository : IRepository<MealDish>
{
    Task<List<MealDish>> GetMealDishesByMealIdsAsync(IEnumerable<int> mealIds, CancellationToken ct);
}
