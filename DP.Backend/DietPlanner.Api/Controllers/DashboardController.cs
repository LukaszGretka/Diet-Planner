using DietPlanner.Api.Extensions;
using DietPlanner.Application.Interfaces.Services;
using DietPlanner.Application.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController(IDashboardService dashboardService) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<DashboardData>> GetDashboardStatsData(CancellationToken ct)
        {
            string userId = HttpContext.GetUserId();

            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await dashboardService.GetDashboardData(userId, ct);

            return Ok(result);
        }
    }
}
