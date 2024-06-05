using DietPlanner.Api.Models.Dashboard;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<DashboardData> GetDashboardData(string userId);
    }
}
