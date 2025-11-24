using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Enums;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.Dashboard
{
    public class GoalService : IGoalService
    {
        private readonly DietPlannerDbContext _databaseContext;

        public GoalService(DietPlannerDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Goals> GetGoalData(string userId, GoalType goalType)
        {
            var goal = await _databaseContext.Goals.Where(x => x.UserId == userId).Where(y => y.GoalType == goalType).FirstOrDefaultAsync();

            if(goal != null)
            {
                return new Goals
                {
                    Value = goal.Value,
                    EstablishmentDate = goal.EstablishmentDate,
                    GoalType = goal.GoalType
                };
            }
            return new Goals();
        }
    }
}
