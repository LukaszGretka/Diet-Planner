using DietPlanner.Application.Models.Dashboard;

namespace DietPlanner.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<DashboardData> GetDashboardData(string userId, CancellationToken ct);
}
