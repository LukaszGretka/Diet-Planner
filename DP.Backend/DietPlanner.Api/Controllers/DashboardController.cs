using DietPlanner.Api.Extensions;
using DietPlanner.Application.DTO.Dashboard;
using DietPlanner.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController(IDashboardService dashboardService) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<DashboardData>> GetDashboardStatsData()
        {
            string userId = HttpContext.GetUserId();

            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            DashboardData dashboardData = await dashboardService.GetDashboardData(userId);

            return Ok(dashboardData);
        }
    }
}
