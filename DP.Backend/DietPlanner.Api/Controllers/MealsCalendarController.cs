using DietPlanner.Api.Extensions;
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

        // new endpoint
        [HttpPost]
        [Route("add-meal-item")]
        public async Task<ActionResult<DailyMealsDto>> AddMealItemRequest([FromBody] AddMealItemRequest addMealItemRequest)
        {
            string userId = HttpContext.GetUserId();

            var result = await _mealsCalendarService.AddMealItem(addMealItemRequest, userId);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(result.Obj);
        }



        //[HttpPut]
        //public async Task<ActionResult<DailyMealsDto>> AddOrUpdateMeal([FromBody] PutMealRequest putMealRequest)
        //{
        //    string userId = HttpContext.GetUserId();

        //    var result = await _mealsCalendarService.AddOrUpdateMeal(putMealRequest, userId);

        //    if (result.Exception != null)
        //    {
        //        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        //    }

        //    return Ok(result.Obj);
        //}

        //[HttpPut]
        //public async Task<ActionResult<DailyMealsDto>> UpdateMealProduct([FromBody] MealProductDto mealProduct)
        //{
        //    string userId = HttpContext.GetUserId();

        //    var result = await _mealsCalendarService.UpdateMealProduct(mealProduct, userId);

        //    if (result.Exception != null)
        //    {
        //        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        //    }

        //    return Ok(result.Obj);
        //}
    }
}
