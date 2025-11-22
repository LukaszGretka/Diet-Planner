using DietPlanner.Api.Extensions;
using DietPlanner.Api.Responses;
using DietPlanner.Api.Services.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardDataResponse>> GetDashboardStatsData()
        {
            string userId = HttpContext.GetUserId();

            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _dashboardService.GetDashboardData(userId);

            return Ok(result);
        }
    }
}
