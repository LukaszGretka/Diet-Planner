using DietPlanner.Application.Models.Dishes;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Models;

namespace DietPlanner.Application.Interfaces.Services;

public interface IDishService
{
    Task<bool> CheckIfExistsAsync(int id, CancellationToken ct);

    Task<Dish?> GetByIdAsync(int id, CancellationToken ct);

    Task<Dish?> GetByNameAsync(string name, CancellationToken ct);

    Task<List<Dish>> GetAllUserDishesAsync(string userId, CancellationToken ct);

    Task<List<Dish>> GetAllAvailableDishesAsync(string userId, CancellationToken ct);

    Task<IEnumerable<DishProduct>> GetDishProductsAsync(int dishId, CancellationToken ct);

    Task<DatabaseActionResult<DishDTO>> CreateAsync(CreateDishRequest dish, string userId, CancellationToken ct);

    Task<DatabaseActionResult> UpdateAsync(UpdateDishRequest dish, string userId, CancellationToken ct);

    Task<DatabaseActionResult> DeleteByIdAsync(int id, string userId, CancellationToken ct);
}
