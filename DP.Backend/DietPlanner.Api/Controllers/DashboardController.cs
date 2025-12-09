using DietPlanner.Api.Extensions;
using DietPlanner.Api.Models.Dashboard;
using DietPlanner.Api.Services.Dashboard;
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
