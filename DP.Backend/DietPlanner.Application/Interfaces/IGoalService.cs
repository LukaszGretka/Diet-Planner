using DietPlanner.Domain.Entities.Goals;
using DietPlanner.Domain.Enums;

namespace DietPlanner.Application.Interfaces
{
    public interface IGoalService
    {
        Task<Goals> GetGoalData(string userId, GoalType goalType);
    }
}