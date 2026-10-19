using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Infrastructure.Repositories;

public class DishRepository(DietPlannerDbContext dbContext, ILogger<DishRepository> logger) : GenericRepository<Dish>(dbContext, logger), IDishRepository
{
    public async Task<Dish?> GetByNameAsync(string name, CancellationToken ct)
    {
        try
        {
            return await dbContext.Dishes.AsNoTracking()
                .FirstOrDefaultAsync(d => d.Name.Equals(name), ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving dish by name {DishName}", name);
            return null;
        }
    }

    public async Task<List<Dish>> GetAllUserDishesAsync(string userId, CancellationToken ct)
    {
        try
        {
            return await dbContext.Dishes.AsNoTracking()
                .Where(d => d.UserId.Equals(userId))
                .ToListAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving dishes for user {UserId}", userId);
            return new List<Dish>();
        }
    }

    public async Task<List<Dish>> GetAllAvailableDishesAsync(string userId, CancellationToken ct)
    {
        try
        {
            return await dbContext.Dishes.AsNoTracking()
                .Where(d => d.UserId.Equals(userId) || d.ExposeToOtherUsers)
                .ToListAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving available dishes for user {UserId}", userId);
            return new List<Dish>();
        }
    }

    public async Task<IEnumerable<DishProduct>> GetDishProductsAsync(int dishId, CancellationToken ct)
    {
        try
        {
            return await dbContext.DishProducts.AsNoTracking()
                .Where(dp => dp.DishId == dishId)
                .Select(x => new DishProduct()
                {
                    Product = x.Product,
                    PortionMultiplier = x.PortionMultiplier,
                })
                .ToListAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving products for dish {DishId}", dishId);
            return Enumerable.Empty<DishProduct>();
        }
    }
}
