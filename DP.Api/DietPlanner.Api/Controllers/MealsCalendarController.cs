using DietPlanner.Api.Models.MealsCalendar;
using DietPlanner.Api.Services.MealsCalendar;
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
        public async Task<ActionResult<DailyMeals>> GetDailyMeals(DateTime date)
        {
           var result = await _mealsCalendarService.GetDailyMeals(date);

            return Ok(result);
        }
    }
}
