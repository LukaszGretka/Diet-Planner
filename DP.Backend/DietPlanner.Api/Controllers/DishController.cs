﻿using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.Extensions;
using DietPlanner.Api.Models.MealsCalendar.DbModel;
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

        [HttpGet("all")]
        public async Task<ActionResult<List<DishDTO>>> GetUserDishes()
        {
            List<DishDTO> dishesDTO = new List<DishDTO>();
            string userId = HttpContext.GetUserId();

            List<Dish> dishes = await _dishService.GetAllUserDishes(userId);

            foreach (Dish dish in dishes)
            {
                var dishProducts = await _dishService.GetDishProducts(dish.Id);

                dishesDTO.Add(new DishDTO
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Description = dish.Description,
                    ImagePath = dish.ImagePath,
                    ExposeToOtherUsers = dish.ExposeToOtherUsers,
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
                });
            }


            return Ok(dishesDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DishDTO>> GetDishById(int id)
        {
            var foundDish = await _dishService.GetById(id);

            if (foundDish is null)
            {
                return NotFound(new { Message = $"Dish with id: {id} no found" });
            }

            return Ok(await AddProductsToDish(foundDish));
        }

        [HttpGet]
        public async Task<ActionResult<DishDTO>> GetDishByName([FromQuery] string dishName)
        {
            var foundDish = await _dishService.GetByName(dishName);

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
            DatabaseActionResult<DishDTO> createDishResult = await _dishService.Create(dishRequest, userId);

            if (createDishResult.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (!createDishResult.Success)
            {
                return BadRequest(createDishResult.Message);
            }

            return CreatedAtAction(nameof(CreateDish), new { id = createDishResult.Obj.Id }, createDishResult.Obj);
        }

        [HttpPatch]
        [ActionName(nameof(UpdateDish))]
        public async Task<IActionResult> UpdateDish([FromBody] PutDishRequest dishRequest)
        {
            string userId = HttpContext.GetUserId();
            DatabaseActionResult updateDishResult = await _dishService.Update(dishRequest, userId);

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
            DatabaseActionResult deleteDishResult = await _dishService.DeleteById(id, userId);

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
            var dishProducts = await _dishService.GetDishProducts(dish.Id);

            return new DishDTO
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                ExposeToOtherUsers = dish.ExposeToOtherUsers,
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
                    PortionMultiplier = dishProduct.PortionMultiplier,
                    CustomizedPortionMultiplier = dishProduct.CustomizedPortionMultiplier
                })
            };
        }
    }
}
