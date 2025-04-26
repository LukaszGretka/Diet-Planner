using DietPlanner.Api.Database.Enums;
using DietPlanner.Api.Database.Models;
using System;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.Dashboard
{
    public interface IGoalService
    {
        Task<Goals> GetGoalData(string userId, GoalType goalType);
    }
}