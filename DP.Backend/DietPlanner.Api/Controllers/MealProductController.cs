
using DietPlanner.Api.Models.MealProductModel;
using DietPlanner.Api.Services.MealProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MealProductController : ControllerBase
    {
        private readonly IMealProductService _mealProductService;

        public MealProductController(IMealProductService mealProductService)
        {
            this._mealProductService = mealProductService;
        }

        [HttpPatch]
        public async Task<IActionResult> PatchMultiplier(UpdateMultiplierRequest request)
        {
            await _mealProductService.UpdatePortionMultiplier(request.Date, request.MealType, request.ProductId, request.PortionMultiplier);

            return Ok();
        }
    }
}
