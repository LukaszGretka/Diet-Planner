using DietPlanner.Api.Database;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.DishService
{
    public class DishService : IDishService
    {
        private readonly ILogger<DishService> _logger;
        private readonly DietPlannerDbContext _databaseContext;

        public DishService(ILogger<DishService> logger, DietPlannerDbContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<Dish> GetById(int id)
        {
            return await _databaseContext.Dishes.FirstOrDefaultAsync(dish => dish.Id == id);
        }

        public async Task<bool> CheckIfExists(int id)
        {
            return await _databaseContext.Dishes.AnyAsync(dish => dish.Id == id);
        }

        public async Task<DatabaseActionResult<Dish>> Create(PutDishRequest request, string userId)
        {
            if (!request.Products.Any())
            {
                return new DatabaseActionResult<Dish>(false, message: "No product were added to the dish");
            }

            var dishProducts = new List<DishProducts>();

            try
            {
                var dishResult = await _databaseContext.Dishes.AddAsync(new Dish
                {
                    Name = request.Name,
                    ImagePath = request.Image,
                    Description = request.Description,
                    UserId = userId,
                    ExposeToOtherUsers = request.ExposeToOtherUsers
                });

                request.Products.ToList().ForEach(dishProduct =>
                {
                    dishProducts.Add(new DishProducts
                    {
                        Product = dishProduct.Product,
                        PortionMultiplier = dishProduct.PortionMultiplier,
                        Dish = dishResult.Entity,
                    });
                });

                _databaseContext.DishProducts.AttachRange(dishProducts);

                await _databaseContext.SaveChangesAsync();

                return new DatabaseActionResult<Dish>(true, obj: dishResult.Entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Dish>(false, exception: ex);
            }
        }

        public async Task<DatabaseActionResult> Update(PutDishRequest requestedDish, string userId)
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

                return new DatabaseActionResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return new DatabaseActionResult(false, exception: ex);
            }
        }

        public async Task<List<Dish>> GetAllUserDishes(string userId)
        {
            return await _databaseContext.Dishes.Where(dish => dish.UserId.Equals(userId)).ToListAsync();
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

        public async Task<DatabaseActionResult<Dish>> DeleteById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
