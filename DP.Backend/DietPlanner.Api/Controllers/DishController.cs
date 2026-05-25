using DietPlanner.Api.Extensions;
using DietPlanner.Application.Interfaces.Services;
using DietPlanner.Application.Models.Dishes;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DishController(IDishService dishService) : Controller
{
    [HttpGet("all")]
    public async Task<ActionResult<List<DishDTO>>> GetUserDishes(CancellationToken ct)
    {
        List<DishDTO> dishesDTO = new List<DishDTO>();
        string userId = HttpContext.GetUserId();

        List<Dish> dishes = await dishService.GetAllAvailableDishesAsync(userId, ct);

        foreach (Dish dish in dishes)
        {
            var dishProducts = await dishService.GetDishProductsAsync(dish.Id, ct);

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
    public async Task<ActionResult<DishDTO>> GetDishById(int id, CancellationToken ct)
    {
        var foundDish = await dishService.GetByIdAsync(id, ct);

        if (foundDish is null)
        {
            return NotFound(new { Message = $"Dish with id: {id} no found" });
        }

        return Ok(await AddProductsToDish(foundDish, ct));
    }

    [HttpGet]
    public async Task<ActionResult<DishDTO>> GetDishByName([FromQuery] string dishName, CancellationToken ct)
    {
        var foundDish = await dishService.GetByNameAsync(dishName, ct);

        if (foundDish is null)
        {
            return NotFound(new { Message = $"Dish with name: {dishName} no found" });
        }

        return Ok(await AddProductsToDish(foundDish, ct));
    }

    [HttpPost]
    [ActionName(nameof(CreateDish))]
    public async Task<IActionResult> CreateDish([FromBody] CreateDishRequest dishRequest, CancellationToken ct)
    {
        string userId = HttpContext.GetUserId();
        DatabaseActionResult<DishDTO> createDishResult = await dishService.CreateAsync(dishRequest, userId, ct);

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
    public async Task<IActionResult> UpdateDish([FromBody] UpdateDishRequest dishRequest, CancellationToken ct)
    {
        string userId = HttpContext.GetUserId();
        DatabaseActionResult updateDishResult = await dishService.UpdateAsync(dishRequest, userId, ct);

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
    public async Task<IActionResult> DeleteDish(int id, CancellationToken ct)
    {
        string userId = HttpContext.GetUserId();
        DatabaseActionResult deleteDishResult = await dishService.DeleteByIdAsync(id, userId, ct);

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

    private async Task<DishDTO> AddProductsToDish(Dish dish, CancellationToken ct)
    {
        //TODO: Having it in the same query with "join" should give better performance.
        var dishProducts = await dishService.GetDishProductsAsync(dish.Id, ct);

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
