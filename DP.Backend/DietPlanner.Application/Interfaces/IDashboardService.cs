using DietPlanner.Application.DTO.Dashboard;

namespace DietPlanner.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardData> GetDashboardData(string userId);
    }
}
