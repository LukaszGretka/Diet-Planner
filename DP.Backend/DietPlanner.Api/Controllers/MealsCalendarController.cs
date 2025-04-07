using DietPlanner.Api.Extensions;
using DietPlanner.Api.Models.MealProductModel;
using DietPlanner.Api.Models.MealsCalendar.DTO;
using DietPlanner.Api.Models.MealsCalendar.Requests;
using DietPlanner.Api.Services.MealProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MealsCalendarController : ControllerBase
    {
        private readonly IMealService _mealsCalendarService;

        public MealsCalendarController(IMealService mealsCalendarService)
        {
            this._mealsCalendarService = mealsCalendarService;
        }

        [HttpGet("{date}")]
        public async Task<ActionResult<DailyMealsDto>> GetDailyMeals(DateTime date)
        {
            string userId = HttpContext.GetUserId();

            var result = await _mealsCalendarService.GetMeals(date, userId);

            return Ok(result);
        }

        [HttpPost]
        [Route("add-meal-item")]
        public async Task<ActionResult<DailyMealsDto>> AddMealItemRequest([FromBody] MealItemRequest addMealItemRequest)
        {
            string userId = HttpContext.GetUserId();

            var result = await _mealsCalendarService.AddMealItem(addMealItemRequest, userId);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(result.Obj);
        }

        [HttpDelete]
        [Route("remove-meal-item")]
        public async Task<ActionResult<DailyMealsDto>> RemoveMealItemRequest([FromBody] MealItemRequest removeMealItemRequest)
        {
            string userId = HttpContext.GetUserId();

            var result = await _mealsCalendarService.RemoveMealItem(removeMealItemRequest, userId);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(result.Obj);
        }

        [HttpPatch]
        [Route("update-meal-item-portion")]
        public async Task<IActionResult> UpdateMealItemPortion(UpdateMealItemPortionRequest request)
        {
            //TODO: split this endpoint to make possibility to update portion of product and dish separately
            var result = await _mealsCalendarService.UpdateMealItemPortion(request);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(result.Success);
        }
    }
}
