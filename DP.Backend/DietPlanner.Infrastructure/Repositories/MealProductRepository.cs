using DietPlanner.Application.Interfaces.Repository;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Infrastructure.Repositories
{
    public class MealProductRepository : GenericRepository<MealProduct>, IMealProductRepository
    {
        private readonly ILogger<MealProductRepository> logger;

        public MealProductRepository(DietPlannerDbContext dbContext,
            ILogger<MealProductRepository> logger) : base(dbContext, logger)
        {
            this.logger = logger;
        }

        public async Task<List<MealProduct>> GetMealProducts(int mealId, CancellationToken ct)
        {
            try
            {
                return await dbContext.MealProducts.Where(mp => mp.MealId == mealId).ToListAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving meal products for mealId {MealId}", mealId);
                return [];
            }
        }
    }
}
