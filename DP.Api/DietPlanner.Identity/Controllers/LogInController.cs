using Microsoft.AspNetCore.Mvc;

namespace DietPlanner.Identity.Controllers
{
    [Route("api/[controller]")]
    public class LogInController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
