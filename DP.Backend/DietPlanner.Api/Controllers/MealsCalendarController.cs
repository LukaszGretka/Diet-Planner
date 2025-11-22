using DietPlanner.Api.Extensions;
using DietPlanner.Api.Requests;
using DietPlanner.Application.DTO;
using DietPlanner.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MealsCalendarController(IMealService mealsCalendarService) : ControllerBase
    {
        [HttpGet("{date}")]
        public async Task<ActionResult<DailyMealsDto>> GetDailyMeals(DateTime date, CancellationToken ct)
        {
            string userId = HttpContext.GetUserId();

            var result = await mealsCalendarService.GetMeals(date, userId, ct);

            return Ok(result.Obj);
        }

        [HttpPost]
        [Route("add-meal-item")]
        public async Task<ActionResult<DailyMealsDto>> AddMealItemRequest([FromBody] MealItemRequest addMealItemRequest, CancellationToken ct)
        {
            string userId = HttpContext.GetUserId();

            var result = await mealsCalendarService.AddMealItem(addMealItemRequest, userId, ct);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(result.Obj);
        }

        [HttpDelete]
        [Route("remove-meal-item")]
        public async Task<ActionResult<DailyMealsDto>> RemoveMealItemRequest([FromBody] MealItemRequest removeMealItemRequest, CancellationToken ct)
        {
            string userId = HttpContext.GetUserId();

            var result = await mealsCalendarService.RemoveMealItem(removeMealItemRequest, userId, ct);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(result.Obj);
        }

        [HttpPatch]
        [Route("update-meal-item-portion")]
        public async Task<IActionResult> UpdateMealItemPortion(UpdateMealItemPortionRequest request, CancellationToken ct)
        {
            string userId = HttpContext.GetUserId();

            var result = await mealsCalendarService.UpdateMealItemPortion(request, userId, ct);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(result.Obj);
        }
    }
}
