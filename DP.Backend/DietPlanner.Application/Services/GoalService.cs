using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Application.Interfaces.Services;
using DietPlanner.Application.Models.Goal;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Application.Services;

public class GoalService(IGoalRepository goalRepository, ILogger<GoalService> logger) : IGoalService
{
    public async Task<GoalDTO?> GetGoalData(string userId, GoalType goalType, CancellationToken ct)
    {

        bool goalDefined = await goalRepository.GoalExistsAsync(userId, ct);

        if (!goalDefined)
        {
            logger.LogTrace("No goals found for user: {UserId}", userId);
            return null;
        }

        Goal? goal = await goalRepository.GetGoalByUserAndTypeAsync(userId, goalType, ct);

        if (goal is null)
        {
            logger.LogError("Goal data not found for user {UserId} and goal type {GoalType}", userId, goalType);
            return null;
        }

        return new GoalDTO
        {
            Value = goal.Value,
            EstablishmentDate = goal.EstablishmentDate,
            GoalType = goal.GoalType
        };
    }
}
