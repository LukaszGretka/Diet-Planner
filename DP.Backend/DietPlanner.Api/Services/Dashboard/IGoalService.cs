using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Enums;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.Dashboard
{
    public interface IGoalService
    {
        Task<Goals> GetGoalData(string userId, GoalType goalType);
    }
}