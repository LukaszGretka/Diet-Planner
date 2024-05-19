
using DietPlanner.Api.Database.Models;
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
    public class DishProductController : ControllerBase
    {
        private readonly IMealProductService _mealProductService;

        public DishProductController(IMealProductService mealProductService)
        {
            this._mealProductService = mealProductService;
        }

        [HttpPatch]
        public async Task<IActionResult> PatchCutomizedMultiplier(UpdateCustomizedMultiplierRequest request)
        {
            await _mealProductService.AddOrUpdateCustomizedPortionMultiplier(request.DishId, request.ProductId, request.MealDishId, request.CustomizedPortionMultiplier);

            return Ok();
        }
    }
}
