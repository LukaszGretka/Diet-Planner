using DietPlanner.Application.DTO.Dishes;
using DietPlanner.Application.Extensions;
using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities.Dishes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Application.Services
{
    public class DishService(ILogger<DishService> logger, IDishRepository dishRepository,
        IDishProductRepository dishProductRepository) : IDishService
    {
        public async Task<Dish?> GetById(int id)
        {
            return await dishRepository.GetByIdAsync(id);
        }

        public async Task<Dish?> GetByName(string name)
        {
            return await dishRepository.GetByNameAsync(name);
        }

        public async Task<bool> CheckIfExists(int id)
        {
            return await dishRepository.GetByIdAsync(id) is not null;
        }

        public async Task<DishDTO?> Create(DishDTO dishDTO, string userId)
        {
            if (!dishDTO.Products.Any())
            {
                logger.LogError("User {UserId} tried to add dish without any products", userId);
                return null;
            }

            var dishProducts = new List<DishProducts>();

            try
            {
                Dish createdDish = await dishRepository.CreateAsync(new Dish
                {
                    Name = dishDTO.Name,
                    ImagePath = dishDTO.ImagePath,
                    Description = dishDTO.Description,
                    UserId = userId,
                    ExposeToOtherUsers = dishDTO.ExposeToOtherUsers
                });

                IEnumerable<DishProductsDTO> dishProductsDTO = dishDTO.Products;

                dishProductsDTO.ToList().ForEach(dishProduct =>
                {
                    dishProducts.Add(new DishProducts
                    {
                        Product = dishProduct.Product,
                        PortionMultiplier = dishProduct.PortionMultiplier,
                        Dish = createdDish
                    });
                });

                await dishProductRepository.AttachRangeAsync(dishProducts);

                return createdDish.ToDataTransferObject(dishProductsDTO);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<DatabaseActionResult> Update(DishDTO requestedDish, string userId)
        {
            try
            {
                Dish existingDish = await _databaseContext.Dishes
                        .SingleOrDefaultAsync(d => d.Id == requestedDish.Id && d.UserId.Equals(userId));

                if (existingDish is null)
                {
                    return new DatabaseActionResult(false, message: $"Dish with id: {requestedDish.Id} not found.");
                }

                existingDish.Name = requestedDish.Name;
                existingDish.Description = requestedDish.Description;
                existingDish.ImagePath = requestedDish.Image;
                existingDish.ExposeToOtherUsers = requestedDish.ExposeToOtherUsers;

                List<DishProducts> dishProducts = await _databaseContext.DishProducts
                     .Where(dishProduct => dishProduct.DishId == requestedDish.Id)
                     .ToListAsync();

                var productsDoAdd = new List<DishProducts>();

                foreach (var requestedDishProduct in requestedDish.Products)
                {
                    var existingDishProduct = dishProducts.FirstOrDefault(dp => dp.ProductId == requestedDishProduct.Product.Id);

                    if (existingDishProduct is null)
                    {
                        productsDoAdd.Add(new DishProducts
                        {
                            Product = requestedDishProduct.Product,
                            PortionMultiplier = requestedDishProduct.PortionMultiplier,
                            Dish = existingDish
                        });
                    }
                    else
                    {
                        existingDishProduct.PortionMultiplier = requestedDishProduct.PortionMultiplier;
                    }
                }

                var productsToRemove = dishProducts.Where(a => !requestedDish.Products.Any(b => b.Product.Id == a.ProductId));

                _databaseContext.DishProducts.AttachRange(productsDoAdd);
                _databaseContext.DishProducts.RemoveRange(productsToRemove);

                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(message: ex.Message);
                return new DatabaseActionResult(false, exception: ex);
            }

            return new DatabaseActionResult(true);
        }

        public async Task<List<Dish>> GetAllUserDishes(string userId)
        {
            return await _databaseContext.Dishes.Where(dish => dish.UserId.Equals(userId)).ToListAsync();
        }

        public async Task<List<Dish>> GetAllAvailableDishes(string userId)
        {
            return await _databaseContext.Dishes.Where(dish => dish.UserId.Equals(userId) || dish.ExposeToOtherUsers).ToListAsync();
        }

        public async Task<IEnumerable<DishProducts>> GetDishProducts(int dishId)
        {
            return await _databaseContext.DishProducts
                .Where(dish => dish.DishId == dishId)
                .Select(x => new DishProducts()
                {
                    Product = x.Product,
                    PortionMultiplier = x.PortionMultiplier,
                }).ToListAsync();
        }

        public async Task<DatabaseActionResult> DeleteById(int id, string userId)
        {
            try
            {
                Dish existingDish = await _databaseContext.Dishes
                        .SingleOrDefaultAsync(d => d.Id == id && d.UserId.Equals(userId));

                if (existingDish is null)
                {
                    return new DatabaseActionResult(false, message: $"Dish with id: {id} not found.");
                }

                List<DishProducts> dishProducts = await _databaseContext.DishProducts
                     .Where(dishProduct => dishProduct.DishId == id)
                     .ToListAsync();
 
               _databaseContext.DishProducts.RemoveRange(dishProducts);
               _databaseContext.Dishes.Remove(existingDish);

                await _databaseContext.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                logger.LogError(message: ex.Message);
                return new DatabaseActionResult(false, exception: ex);
            }

            return new DatabaseActionResult(true);
        }
    }
}
