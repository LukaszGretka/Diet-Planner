using DietPlanner.Domain.Enums;

namespace DietPlanner.Api.Services.Dashboard
{
    public interface IGoalService
    {
        Task<Goals> GetGoalData(string userId, GoalType goalType);
    }
}