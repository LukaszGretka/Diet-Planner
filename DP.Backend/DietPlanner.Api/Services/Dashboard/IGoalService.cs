using DietPlanner.Application.Models.Goal;
using DietPlanner.Domain.Enums;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.Dashboard
{
    public interface IGoalService
    {
        Task<GoalDTO?> GetGoalData(string userId, GoalType goalType);
    }
}