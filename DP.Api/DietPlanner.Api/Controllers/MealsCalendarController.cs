using DietPlanner.Api.Extensions;
using DietPlanner.Api.Models.MealsCalendar.DTO;
using DietPlanner.Api.Services.MealsCalendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MealsCalendarController : ControllerBase
    {
        private readonly IMealsCalendarService _mealsCalendarService;

        public MealsCalendarController(IMealsCalendarService mealsCalendarService)
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
        public async Task<ActionResult<DailyMealsDto>> AddOrUpdateMeal([FromBody] MealByDay mealByDate)
        {
            string userId = HttpContext.GetUserId();

            var result = await _mealsCalendarService.AddOrUpdateMeal(mealByDate, userId);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(result.Obj);
        }
    }
}
