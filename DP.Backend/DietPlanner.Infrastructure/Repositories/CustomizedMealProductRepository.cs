using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Infrastructure.Repositories;

public class CustomizedMealProductRepository : GenericRepository<CustomizedMealProducts>, ICustomizedMealProductRepository
{
    private readonly ILogger<CustomizedMealProductRepository> logger;

    public CustomizedMealProductRepository(DietPlannerDbContext dbContext,
        ILogger<CustomizedMealProductRepository> logger) : base(dbContext, logger)
    {
        this.logger = logger;
    }

    public Task<decimal?> GetPortionMultiplierAsync(int mealProductId, CancellationToken ct)
    {
        try
        {
            return dbContext.CustomizedMealProducts.Where(cmp => cmp.MealProductId == mealProductId)
                    .Select(cmp => cmp.CustomizedPortionMultiplier)
                    .FirstOrDefaultAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving portion multiplier for MealProductId {MealProductId}", mealProductId);
            return null;
        }
    }
}
