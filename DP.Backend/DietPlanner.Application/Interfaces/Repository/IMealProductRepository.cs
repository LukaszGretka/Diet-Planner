using DietPlanner.Application.Interfaces.Common;
using DietPlanner.Domain.Entities;

namespace DietPlanner.Application.Interfaces.Repository
{
    public interface IMealProductRepository : IGenericRepository<MealProduct>
    {
        public Task<List<MealProduct>> GetMealProducts(int mealId, CancellationToken ct);
    }
}
