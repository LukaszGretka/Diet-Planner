using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Infrastructure.Repositories;

public class ProductRepository(DietPlannerDbContext dbContext, ILogger<ProductRepository> logger) : GenericRepository<Product>(dbContext, logger), IProductRepository
{
    public async Task<Product?> GetByNameAsync(string name, CancellationToken ct)
    {
        try
        {
            return await dbContext.Products.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name.Equals(name), ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving product by name {ProductName}", name);
            return null;
        }
    }
}
