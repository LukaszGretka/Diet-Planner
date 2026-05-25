using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Enums;
using DietPlanner.Domain.Repositories;

namespace DietPlanner.Application.Interfaces.Repositories;

public interface IGoalRepository : IUserRepository<Goal>
{
    Task<bool> GoalExistsAsync(string userId, CancellationToken ct);
    Task<Goal?> GetGoalByUserAndTypeAsync(string userId, GoalType goalType, CancellationToken ct);
}
