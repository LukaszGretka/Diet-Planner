using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Infrastructure.Repositories;

public class CustomizedMealDishRepository(DietPlannerDbContext dbContext,
    ILogger<CustomizedMealDishRepository> logger) : GenericRepository<CustomizedMealDish>(dbContext, logger), ICustomizedMealDishRepository
{
    public async Task<CustomizedMealDish?> GetByMealDishIdAndDishProductIdAsync(int mealDishId, int dishProductId, CancellationToken ct)
    {
        try
        {
            return await dbContext.CustomizedMealDishes
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.MealDishId == mealDishId && e.DishProductId == dishProductId, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving CustomizedMealDish for MealDishId {MealDishId} and DishProductId {DishProductId}", mealDishId, dishProductId);
            return null;
        }
    }
}
