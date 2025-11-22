using DietPlanner.Api.Extensions;
using DietPlanner.Api.Models.MealsCalendar.DbModel;
using DietPlanner.Api.Requests;
using DietPlanner.Application.DTO.Dishes;
using DietPlanner.Application.Interfaces;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Entities.Dishes;
using DietPlanner.Domain.Entities.Products;
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
    public class DishController(IDishService dishService) : Controller
    {
        [HttpGet("all")]
        public async Task<ActionResult<List<DishDTO>>> GetUserDishes()
        {
            List<DishDTO> dishesDTO = [];
            string userId = HttpContext.GetUserId();

            List<Dish> dishes = await dishService.GetAllAvailableDishes(userId);

            foreach (Dish dish in dishes)
            {
                var dishProducts = await dishService.GetDishProducts(dish.Id);

                dishesDTO.Add(new DishDTO
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Description = dish.Description,
                    ImagePath = dish.ImagePath,
                    ExposeToOtherUsers = dish.ExposeToOtherUsers,
                    IsOwner = userId.Equals(dish.UserId),
                    Products = dishProducts.Select(dishProduct => new DishProductsDTO
                    {
                        Product = new Product
                        {
                            Id = dishProduct.Product.Id,
                            Name = dishProduct.Product.Name,
                            Description = dishProduct.Product.Description,
                            BarCode = dishProduct.Product.BarCode,
                            ImagePath = dishProduct.Product.ImagePath,
                            Calories = dishProduct.Product.Calories * (float)dishProduct.PortionMultiplier,
                            Carbohydrates = dishProduct.Product.Carbohydrates * (float)dishProduct.PortionMultiplier,
                            Proteins = dishProduct.Product.Proteins * (float)dishProduct.PortionMultiplier,
                            Fats = dishProduct.Product.Fats * (float)dishProduct.PortionMultiplier,
                        },
                        PortionMultiplier = dishProduct.PortionMultiplier,
                        CustomizedPortionMultiplier = null      
                    })
                });
            }


            return Ok(dishesDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DishDTO>> GetDishById(int id)
        {
            var foundDish = await dishService.GetById(id);

            if (foundDish is null)
            {
                return NotFound(new { Message = $"Dish with id: {id} no found" });
            }

            return Ok(await AddProductsToDish(foundDish));
        }

        [HttpGet]
        public async Task<ActionResult<DishDTO>> GetDishByName([FromQuery] string dishName)
        {
            var foundDish = await dishService.GetByName(dishName);

            if (foundDish is null)
            {
                return NotFound(new { Message = $"Dish with name: {dishName} no found" });
            }

            return Ok(await AddProductsToDish(foundDish));
        }

        [HttpPost]
        [ActionName(nameof(CreateDish))]
        public async Task<IActionResult> CreateDish([FromBody] PutDishRequest dishRequest)
        {
            string userId = HttpContext.GetUserId();
            DishDTO dishDTO = await dishService.Create(dishRequest, userId);

            if (dishDTO is null)
            {
                return NotFound(new { Message = "Dish could not be created" });
            }

            return CreatedAtAction(nameof(CreateDish), new { id = dishDTO.Id }, dishDTO);
        }

        [HttpPatch]
        [ActionName(nameof(UpdateDish))]
        public async Task<IActionResult> UpdateDish([FromBody] PutDishRequest dishRequest)
        {
            string userId = HttpContext.GetUserId();
            DatabaseActionResult updateDishResult = await dishService.Update(dishRequest, userId);

            if (updateDishResult.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (!updateDishResult.Success)
            {
                return BadRequest(updateDishResult.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ActionName(nameof(DeleteDish))]
        public async Task<IActionResult> DeleteDish(int id)
        {
            string userId = HttpContext.GetUserId();
            DatabaseActionResult deleteDishResult = await dishService.DeleteById(id, userId);

            if (deleteDishResult.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (!deleteDishResult.Success)
            {
                return BadRequest(deleteDishResult.Message);
            }

            return Ok();
        }

        private async Task<DishDTO> AddProductsToDish(Dish dish)
        {
            //TODO: Having it in the same query with "join" should give better performance.
            var dishProducts = await dishService.GetDishProducts(dish.Id);

            return new DishDTO
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                ExposeToOtherUsers = dish.ExposeToOtherUsers,
                IsOwner = HttpContext.GetUserId().Equals(dish.UserId),
                ImagePath = dish.ImagePath,
                Products = dishProducts.Select(dishProduct => new DishProductsDTO
                {
                    Product = new Product
                    {
                        Id = dishProduct.Product.Id,
                        Name = dishProduct.Product.Name,
                        Description = dishProduct.Product.Description,
                        BarCode = dishProduct.Product.BarCode,
                        ImagePath = dishProduct.Product.ImagePath,
                        Calories = dishProduct.Product.Calories * (float)dishProduct.PortionMultiplier,
                        Carbohydrates = dishProduct.Product.Carbohydrates * (float)dishProduct.PortionMultiplier,
                        Proteins = dishProduct.Product.Proteins * (float)dishProduct.PortionMultiplier,
                        Fats = dishProduct.Product.Fats * (float)dishProduct.PortionMultiplier,
                    },
                    PortionMultiplier = dishProduct.PortionMultiplier
                })
            };
        }
    }
}
