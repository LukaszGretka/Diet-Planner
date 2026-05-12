using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Infrastructure.Repositories
{
    public class MealDishRepository(DietPlannerDbContext dbContext,
        ILogger<MealDishRepository> logger) : GenericRepository<MealDish>(dbContext, logger), IMealDishRepository
    {
        public async Task<List<MealDish>> GetAllAsync(int mealId, CancellationToken ct)
        {
            try
            {
                return await dbContext.MealDishes
                    .Where(md => md.MealId == mealId)
                    .Include(md => md.Dish)
                    .AsNoTracking()
                    .ToListAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving MealDishes for MealId {MealId}", mealId);
                return [];
            }
        }
    }
}
