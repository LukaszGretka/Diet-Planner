using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Infrastructure.Repositories
{
    public class MealRepository(DietPlannerDbContext dbContext, ILogger<MealRepository> logger) 
        : GenericRepository<Meal>(dbContext, logger), IMealRepository
    {
        public async Task<Meal> GetMealByDateAndTypeAsync(DateTime date, int mealType, string userId, CancellationToken ct)
        {
            try
            {
                return await dbContext.Meals
                    .FirstOrDefaultAsync(m => 
                        m.Date.Date == date.Date && 
                        m.MealType == mealType && 
                        m.UserId == userId, 
                    ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving meal by date and type");
                return null;
            }
        }

        public async Task<List<Meal>> GetMealsByDateAsync(DateTime date, string userId, CancellationToken ct)
        {
            try
            {
                return await dbContext.Meals
                    .Where(m => m.UserId == userId && m.Date.Date == date.Date)
                    .ToListAsync(cancellationToken: ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving meals by date");
                return new List<Meal>();
            }
        }

        public async Task<List<Meal>> GetMealsByUserAndDateRangeAsync(string userId, DateTime fromDate, DateTime toDate, CancellationToken ct)
        {
            try
            {
                return await dbContext.Meals
                    .Where(m => m.UserId == userId && m.Date.Date >= fromDate.Date && m.Date.Date <= toDate.Date)
                    .ToListAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving meals by user and date range");
                return new List<Meal>();
            }
        }

        public async Task<Meal> CreateMealAsync(Meal meal, CancellationToken ct)
        {
            try
            {
                return await CreateAsync(meal, ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating meal");
                return null;
            }
        }

        public async Task<bool> RemoveMealAsync(int mealId, CancellationToken ct)
        {
            try
            {
                var meal = await GetByIdAsync(mealId, ct);
                if (meal == null)
                    return false;

                return await DeleteAsync(meal, ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing meal");
                return false;
            }
        }
    }
}
