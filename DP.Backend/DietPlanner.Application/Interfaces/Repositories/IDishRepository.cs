using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Repositories;

namespace DietPlanner.Application.Interfaces.Repositories;

public interface IDishRepository : IRepository<Dish>
{
    Task<Dish?> GetByNameAsync(string name, CancellationToken ct);

    Task<List<Dish>> GetAllUserDishesAsync(string userId, CancellationToken ct);

    Task<List<Dish>> GetAllAvailableDishesAsync(string userId, CancellationToken ct);

    Task<IEnumerable<DishProduct>> GetDishProductsAsync(int dishId, CancellationToken ct);
}
