using DietPlanner.Api.Models.Dto.MealsCalendar;
using DietPlanner.Api.Models.MealsCalendar;
using DietPlanner.Api.Services.MealsCalendar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealsCalendarController : ControllerBase
    {
        private readonly IMealsCalendarService _mealsCalendarService;

        public MealsCalendarController(IMealsCalendarService mealsCalendarService)
        {
            this._mealsCalendarService = mealsCalendarService;
        }

        [HttpGet]
        [Route("{date}")]
        public async Task<ActionResult<DailyMealsDTO>> GetDailyMeals(DateTime date)
        {
            var result = await _mealsCalendarService.GetDailyMeals(date);

            return Ok(result);
        }

        [HttpPost]
        [Route("{date}")]
        public async Task<ActionResult<DailyMealsDTO>> GetDailyMeals(DateTime date, [FromBody] Meal meal)
        {
            var result = await _mealsCalendarService.AddMeal(date, meal);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction(nameof(GetDailyMeals), result.Obj);
        }

    }
}
