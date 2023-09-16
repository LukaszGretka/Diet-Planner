using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.Extensions;
using DietPlanner.Api.Services.DishService;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<List<DishDTO>>> GetUserDishes()
        {
            string userId = HttpContext.GetUserId();

            List<Dish> dishes = await _dishService.GetAllUserDishes(userId);

            return Ok(dishes.Select(dish =>
                new DishDTO
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Description = dish.Description,
                    ImagePath = dish.ImagePath,
                    ExposeToOtherUsers = dish.ExposeToOtherUsers
                }
            ));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DishDTO>> GetDishById(int id)
        {
            var foundDish = await _dishService.GetById(id);

            if (foundDish is null)
            {
                return NotFound(new { Message = $"Dish with id: {id} no found" });
            }

            //TODO: Having it in the same query with "join" should give better performance.
            var products = await _dishService.GetDishProducts(foundDish.Id);

            return Ok(new DishDTO
            {
                Id = foundDish.Id,
                Name = foundDish.Name,
                Description = foundDish.Description,
                ExposeToOtherUsers = foundDish.ExposeToOtherUsers,
                ImagePath = foundDish.ImagePath,
                Products = products.Select(p => new DishProductsDTO
                {
                    Product = p.Product,
                    PortionMultiplier = p.PortionMultiplier
                })
            });
        }

        [HttpPost]
        [ActionName(nameof(CreateDish))]
        public async Task<IActionResult> CreateDish([FromBody] PutDishRequest dishRequest)
        {
            string userId = HttpContext.GetUserId();
            DatabaseActionResult<Dish> createDishResult = await _dishService.Create(dishRequest, userId);

            if (createDishResult.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (!createDishResult.Success)
            {
                return BadRequest(createDishResult.Message);
            }

            Dish createdDish = createDishResult.Obj;

            return CreatedAtAction(nameof(CreateDish), new { id = createdDish.Id },
                new DishDTO
                {
                    Id = createdDish.Id,
                    Name = createdDish.Name,
                    Description = createdDish.Description,
                    ExposeToOtherUsers = createdDish.ExposeToOtherUsers,
                    ImagePath = createdDish.ImagePath
                });
        }

        [HttpPatch]
        [ActionName(nameof(UpdateDish))]
        public async Task<IActionResult> UpdateDish([FromBody] PutDishRequest dishRequest)
        {
            string userId = HttpContext.GetUserId();
            DatabaseActionResult createDishResult = await _dishService.Update(dishRequest, userId);

            if (createDishResult.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpGet("{dishId}/products")]
        public async Task<ActionResult<List<DishProductsDTO>>> GetDishProducts(int dishId)
        {
            IEnumerable<DishProducts> dishProducts = await _dishService.GetDishProducts(dishId);

            return Ok(dishProducts.Select(dishProduct => new DishProductsDTO
            {
                Product = dishProduct.Product,
                PortionMultiplier = dishProduct.PortionMultiplier
            }));
        }
    }
}
