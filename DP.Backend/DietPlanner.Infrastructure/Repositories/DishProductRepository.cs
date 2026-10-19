using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Infrastructure.Repositories;

public class DishProductRepository(DietPlannerDbContext dbContext,
    ILogger<DishProductRepository> logger) : GenericRepository<DishProduct>(dbContext, logger), IDishProductRepository
{
    public async Task<bool> IsProductAssignedToDishAsync(int productId, CancellationToken ct)
    {
        try
        {
            return await dbContext.DishProducts.AsNoTracking()
                .AnyAsync(dp => dp.ProductId == productId, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking if product {ProductId} is assigned to a dish", productId);
            return false;
        }
    }
}
