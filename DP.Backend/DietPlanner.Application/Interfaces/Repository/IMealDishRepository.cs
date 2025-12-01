using DietPlanner.Application.Interfaces.Common;
using DietPlanner.Domain.Entities;

namespace DietPlanner.Application.Interfaces.Repository
{
    public interface IMealDishRepository : IGenericRepository<MealDish>
    {
        public Task<List<MealDish>> GetAllAsync(int mealId, CancellationToken ct);
    }
}
