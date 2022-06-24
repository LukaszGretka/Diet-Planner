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

        [HttpGet("{date}")]
        public async Task<ActionResult<DailyMealsDTO>> GetDailyMeals(DateTime date)
        {
            var result = await _mealsCalendarService.GetDailyMeals(date);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<DailyMealsDTO>> AddDailyMeal([FromBody] MealByDay mealByDate)
        {
            var result = await _mealsCalendarService.AddMeal(mealByDate);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok();
            //return CreatedAtAction(nameof(GetDailyMeals), result.Obj);
        }

    }
}
