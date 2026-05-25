using DietPlanner.Domain.Entities;

namespace DietPlanner.Application.Interfaces.Repositories;

public interface IMealRepository
{
    Task<Meal> GetMealByDateAndTypeAsync(DateTime date, int mealType, string userId, CancellationToken ct);

    Task<List<Meal>> GetMealsByDateAsync(DateTime date, string userId, CancellationToken ct);

    Task<List<Meal>> GetMealsByUserAndDateRangeAsync(string userId, DateTime fromDate, DateTime toDate, CancellationToken ct);

    Task<Meal> CreateMealAsync(Meal meal, CancellationToken ct);

    Task<bool> RemoveMealAsync(int mealId, CancellationToken ct);
}
