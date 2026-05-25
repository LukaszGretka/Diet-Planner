using DietPlanner.Application.Models.Goal;
using DietPlanner.Domain.Enums;

namespace DietPlanner.Application.Interfaces.Services;

public interface IGoalService
{
    Task<GoalDTO?> GetGoalData(string userId, GoalType goalType, CancellationToken ct);
}
