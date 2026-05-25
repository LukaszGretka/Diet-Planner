using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Enums;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Infrastructure.Repositories;

public class GoalRepository(DietPlannerDbContext dbContext, ILogger<GoalRepository> logger)
    : GenericUserRepository<Goal>(dbContext), IGoalRepository
{
    public async Task<bool> GoalExistsAsync(string userId, CancellationToken ct)
    {
        try
        {
            return await dbContext.Goals.AsNoTracking()
                .AnyAsync(x => x.UserId == userId, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking if goal exists for user {UserId}", userId);
            return false;
        }
    }

    public async Task<Goal?> GetGoalByUserAndTypeAsync(string userId, GoalType goalType, CancellationToken ct)
    {
        try
        {
            return await dbContext.Goals.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.GoalType == goalType, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving goal for user {UserId} and goal type {GoalType}", userId, goalType);
            return null;
        }
    }
}
