using DietPlanner.Application.Models.Goal;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Enums;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.Dashboard
{
    public class GoalService(DietPlannerDbContext databaseContext, ILogger<GoalService> logger) : IGoalService
    {
        //TODO: instead of using _databaseContext, create a GoalRepository (infrastructure) and use it here
        private readonly DietPlannerDbContext _databaseContext = databaseContext;

        public async Task<GoalDTO?> GetGoalData(string userId, GoalType goalType)
        {
            //to be replaced with repository call
            Goal goal = await _databaseContext.Goals
                                .Where(x => x.UserId == userId)
                                .Where(y => y.GoalType == goalType)
                                .FirstOrDefaultAsync();

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
}
