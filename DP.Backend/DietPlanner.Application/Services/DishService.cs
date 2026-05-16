using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Application.Interfaces.Services;
using DietPlanner.Application.Models.Dishes;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Application.Services;

public class DishService(
    IDishRepository dishRepository,
    IDishProductRepository dishProductRepository,
    ILogger<DishService> logger) : IDishService
{
    public async Task<bool> CheckIfExistsAsync(int id, CancellationToken ct)
    {
        return await dishRepository.GetByIdAsync(id, ct) is not null;
    }

    public async Task<Dish?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await dishRepository.GetByIdAsync(id, ct);
    }

    public async Task<Dish?> GetByNameAsync(string name, CancellationToken ct)
    {
        return await dishRepository.GetByNameAsync(name, ct);
    }

    public async Task<List<Dish>> GetAllUserDishesAsync(string userId, CancellationToken ct)
    {
        return await dishRepository.GetAllUserDishesAsync(userId, ct);
    }

    public async Task<List<Dish>> GetAllAvailableDishesAsync(string userId, CancellationToken ct)
    {
        return await dishRepository.GetAllAvailableDishesAsync(userId, ct);
    }

    public async Task<IEnumerable<DishProduct>> GetDishProductsAsync(int dishId, CancellationToken ct)
    {
        return await dishRepository.GetDishProductsAsync(dishId, ct);
    }

    public async Task<DatabaseActionResult<DishDTO>> CreateAsync(CreateDishRequest request, string userId, CancellationToken ct)
    {
        if (request.Products.Count == 0)
        {
            return new DatabaseActionResult<DishDTO>(false, message: "No products were added to the dish");
        }

        var dishProducts = new List<DishProduct>();

        try
        {
            var newDish = new Dish
            {
                Name = request.Name,
                ImagePath = request.Image,
                Description = request.Description,
                UserId = userId,
                ExposeToOtherUsers = request.ExposeToOtherUsers
            };

            var createdDish = await dishRepository.CreateAsync(newDish, ct);
            if (createdDish is null)
            {
                return new DatabaseActionResult<DishDTO>(false, "Failed to create dish");
            }

            request.Products.ToList().ForEach(dishProduct =>
            {
                dishProducts.Add(new DishProduct
                {
                    Product = dishProduct.Product,
                    PortionMultiplier = dishProduct.PortionMultiplier,
                    Dish = createdDish
                });
            });

            // We need to add dish products to the repository
            foreach (var dishProduct in dishProducts)
            {
                await dishProductRepository.CreateAsync(dishProduct, ct);
            }

            return new DatabaseActionResult<DishDTO>(true, obj: new DishDTO
            {
                Id = createdDish.Id,
                Name = createdDish.Name,
                Description = createdDish.Description,
                ExposeToOtherUsers = createdDish.ExposeToOtherUsers,
                ImagePath = createdDish.ImagePath,
                Products = dishProducts.Select(dp => new DishProductsDTO
                {
                    Product = dp.Product,
                    PortionMultiplier = dp.PortionMultiplier
                }),
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating dish");
            return new DatabaseActionResult<DishDTO>(false, exception: ex);
        }
    }

    public async Task<DatabaseActionResult> UpdateAsync(UpdateDishRequest requestedDish, string userId, CancellationToken ct)
    {
        try
        {
            var existingDish = await dishRepository.GetByIdAsync(requestedDish.Id, ct);
            if (existingDish is null || !existingDish.UserId.Equals(userId))
            {
                return new DatabaseActionResult(false, message: $"Dish with id: {requestedDish.Id} not found.");
            }

            existingDish.Name = requestedDish.Name;
            existingDish.Description = requestedDish.Description;
            existingDish.ImagePath = requestedDish.Image;
            existingDish.ExposeToOtherUsers = requestedDish.ExposeToOtherUsers;

            var updatedDish = await dishRepository.UpdateAsync(existingDish, ct);
            if (updatedDish is null)
            {
                return new DatabaseActionResult(false, "Failed to update dish");
            }

            var existingDishProducts = (await dishRepository.GetDishProductsAsync(requestedDish.Id, ct)).ToList();
            var productsToAdd = new List<DishProduct>();

            foreach (var requestedDishProduct in requestedDish.Products)
            {
                var existingDishProduct = existingDishProducts.FirstOrDefault(dp => dp.ProductId == requestedDishProduct.Product.Id);

                if (existingDishProduct is null)
                {
                    productsToAdd.Add(new DishProduct
                    {
                        Product = requestedDishProduct.Product,
                        PortionMultiplier = requestedDishProduct.PortionMultiplier,
                        Dish = updatedDish
                    });
                }
                else
                {
                    existingDishProduct.PortionMultiplier = requestedDishProduct.PortionMultiplier;
                    await dishProductRepository.UpdateAsync(existingDishProduct, ct);
                }
            }

            var productsToRemove = existingDishProducts.Where(a => !requestedDish.Products.Any(b => b.Product.Id == a.ProductId));

            foreach (var product in productsToAdd)
            {
                await dishProductRepository.CreateAsync(product, ct);
            }

            foreach (var product in productsToRemove)
            {
                await dishProductRepository.DeleteAsync(product, ct);
            }

            return new DatabaseActionResult(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating dish with id {DishId}", requestedDish.Id);
            return new DatabaseActionResult(false, exception: ex);
        }
    }

    public async Task<DatabaseActionResult> DeleteByIdAsync(int id, string userId, CancellationToken ct)
    {
        try
        {
            var existingDish = await dishRepository.GetByIdAsync(id, ct);
            if (existingDish is null || !existingDish.UserId.Equals(userId))
            {
                return new DatabaseActionResult(false, message: $"Dish with id: {id} not found.");
            }

            var dishProducts = (await dishRepository.GetDishProductsAsync(id, ct)).ToList();

            foreach (var dishProduct in dishProducts)
            {
                await dishProductRepository.DeleteAsync(dishProduct, ct);
            }

            var deleted = await dishRepository.DeleteAsync(existingDish, ct);
            if (!deleted)
            {
                return new DatabaseActionResult(false, "Failed to delete dish");
            }

            return new DatabaseActionResult(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting dish with id {DishId}", id);
            return new DatabaseActionResult(false, exception: ex);
        }
    }
}
