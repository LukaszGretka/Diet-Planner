using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.Models.MealsCalendar.DbModel;
using DietPlanner.Api.Services.DishService;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DishController : Controller
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet]
        public async Task<ActionResult<Dish>> GetDishById(int id) // TODO: change for DishDTO after tests
        {
            var dish =  await _dishService.GetById(id);

            if(dish is null)
            {
                return NotFound(new { Message = $"Dish with id: {id} no found" });
            }

            return dish;
        }

        [HttpPost]
        [ActionName(nameof(CreateDish))]
        public async Task<IActionResult> CreateDish([FromBody] CreateDishRequest createDishRequest)
        {
            DatabaseActionResult<Dish> result = await _dishService.Create(createDishRequest);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction(nameof(CreateDish), new { id = result.Obj.Id }, result.Obj);
        }
    }
}
