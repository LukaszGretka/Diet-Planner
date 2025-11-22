using DietPlanner.Domain.Entities.Dishes;

namespace DietPlanner.Application.Interfaces.Repositories
{
    public interface IDishRepository : IGenericRepository<Dish>
    {
        Task<Dish?> GetByNameAsync(string name);
    }
}
