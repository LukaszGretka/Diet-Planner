using DietPlanner.Api.Database;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.DTO.Products;
using DietPlanner.Api.Enums;
using DietPlanner.Api.Extensions;
using DietPlanner.Api.Models.MealsCalendar.DbModel;
using DietPlanner.Api.Models.MealsCalendar.DTO;
using DietPlanner.Api.Models.MealsCalendar.Requests;
using DietPlanner.Api.Services.MealProductService;
using DietPlanner.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealsCalendar
{
    public class MealService : IMealService
    {
        private readonly ILogger<MealService> _logger;
        private readonly DietPlannerDbContext _databaseContext;

        public MealService(ILogger<MealService> logger, DietPlannerDbContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<List<MealDto>> GetMeals(DateTime date, string userId)
        {
            string formattedDate = date.ToDatabaseDateFormat();

            var meals = await _databaseContext.Meals
                .Where(m => m.UserId == userId && m.Date.Date == date.Date)
                .ToListAsync();

            var mealDishes = await _databaseContext.MealDishes
                .Where(md => meals.Select(m => m.Id).Contains(md.MealId))
                .ToListAsync();

            var mealProducts = await _databaseContext.MealProducts
                .Where(md => meals.Select(m => m.Id).Contains(md.MealId))
                .Select(x => _databaseContext.Products.Single(p => x.Id == p.Id))
                .ToListAsync();

            var dishes = await _databaseContext.Dishes
                .Where(d => mealDishes.Select(md => md.DishId).Contains(d.Id))
                .ToListAsync();

            var dishProducts = await _databaseContext.DishProducts
                .Where(dp => dishes.Select(d => d.Id).Contains(dp.DishId))
                .ToListAsync();

            var products = await _databaseContext.Products
                .Where(p => dishProducts.Select(dp => dp.ProductId).Contains(p.Id))
                .ToListAsync();

            var customizedDishProducts = await _databaseContext.CustomizedDishProducts
                .Where(cdp => mealDishes.Select(md => md.Id).Contains(cdp.MealDishId))
                .ToListAsync();


            var mealDtos = meals
                .GroupBy(m => new { m.MealType })
                .Select(g => new MealDto
                {
                    MealType = (MealType)g.Key.MealType,
                    Products = g.SelectMany(m => _databaseContext.MealProducts.Where(mp => mp.MealId == m.Id))
                        .Select(mp => mp.ProductId).SelectMany(pId => _databaseContext.Products.Where(p => p.Id == pId)).
                            Select(product => new ProductDTO
                            {
                                Id = product.Id,
                                Name = product.Name,
                                Description = product.Description,
                                ImagePath = product.ImagePath,
                                ItemType = ItemType.Product,
                                BarCode = product.BarCode,
                                Calories = (float)product.Calories,
                                Carbohydrates = (float)product.Carbohydrates,
                                Proteins = (float)product.Proteins,
                                Fats = (float)product.Fats
                            }).ToList(),
                    Dishes = g.SelectMany(m => mealDishes.Where(md => md.MealId == m.Id)
                        .Select(md => new DishDTO
                        {
                            Id = md.DishId,
                            MealDishId = md.Id,
                            Name = dishes.First(d => d.Id == md.DishId).Name,
                            Description = dishes.First(d => d.Id == md.DishId).Description,
                            ImagePath = dishes.First(d => d.Id == md.DishId).ImagePath,
                            ExposeToOtherUsers = dishes.First(d => d.Id == md.DishId).ExposeToOtherUsers,
                            Products = dishProducts.Where(dp => dp.DishId == md.DishId)
                                .Select(dp => new DishProductsDTO
                                {
                                    Product = products.First(p => p.Id == dp.ProductId),
                                    PortionMultiplier = dp.PortionMultiplier,
                                    CustomizedPortionMultiplier = customizedDishProducts
                                        .Where(cdp => cdp.MealDishId == md.Id && cdp.DishProductId == dp.Id)
                                        .Select(cdp => (decimal?)cdp.CustomizedPortionMultiplier)
                                        .FirstOrDefault()
                                }).ToList()
                        })).ToList()
                }).ToList();

            return mealDtos;
        }

        public async Task<DatabaseActionResult<Meal>> AddMealItem(AddMealItemRequest addMealItemRequest, string userId)
        {
            Meal foundMeal = await _databaseContext.Meals
                .Where(meal => meal.Date.Date == addMealItemRequest.Date.Date && meal.MealType == (int)addMealItemRequest.MealType)
                .SingleOrDefaultAsync();

            foundMeal ??= CreateNewMeal(addMealItemRequest.Date, addMealItemRequest.MealType, userId);

            return addMealItemRequest.ItemType switch
            {
                ItemType.Product => await AddProductToMeal(foundMeal, addMealItemRequest.ItemId),
                ItemType.Dish => await AddDishToMeal(foundMeal, addMealItemRequest.ItemId),
                _ => new DatabaseActionResult<Meal>(false, obj: null),
            };
        }

        private async Task<DatabaseActionResult<Meal>> AddProductToMeal(Meal meal, int itemId)
        {
            Product product = _databaseContext.Products.Find(itemId);

            if (product is null)
            {
                return new DatabaseActionResult<Meal>(false, obj: null, message: $"Product with id {itemId} can't be found");
            }

            _databaseContext.MealProducts.Add(new MealProduct
            {
                ProductId = itemId,
                Meal = meal,
                Product = product,
                MealId = meal.Id
            });

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Meal>(false, exception: ex);
            }

            return new DatabaseActionResult<Meal>(true, obj: meal);
        }

        private async Task<DatabaseActionResult<Meal>> AddDishToMeal(Meal meal, int itemId)
        {
            Dish dish = _databaseContext.Dishes.Find(itemId);

            if (dish is null)
            {
                return new DatabaseActionResult<Meal>(false, obj: null, message: $"Dish with id {itemId} can't be found");
            }

            _databaseContext.MealDishes.Add(new MealDish
            {
                Meal = meal,
                Dish = dish
            });

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Meal>(false, exception: ex);
            }

            return new DatabaseActionResult<Meal>(true, obj: meal);
        }

        private static Meal CreateNewMeal(DateTime date, MealType mealType, string userId)
        {
            return new()
            {
                Date = date,
                MealType = (int)mealType,
                UserId = userId
            };
        }

        public async Task<DatabaseActionResult<Meal>> AddOrUpdateMeal(PutMealRequest mealRequest, string userId)
        {
            var dishIds = mealRequest.Dishes.Select(dish => dish.Id).ToList();

            if (!dishIds.Any())
            {
                return await RemoveMealEntry(mealRequest, mealRequest.Date);
            }

            var existingMeal = await _databaseContext.Meals
                .Where(meal => meal.Date.Date == mealRequest.Date.Date
                    && meal.MealType == (int)mealRequest.MealType)
                .SingleOrDefaultAsync();


            if (existingMeal is not null)
            {
                return await UpdateMealDishes(existingMeal, mealRequest);
            }

            Meal newMeal = new()
            {
                Date = mealRequest.Date.Date,
                MealType = (int)mealRequest.MealType,
                UserId = userId
            };

            List<MealDish> newMealDishes = new();

            foreach (var dishDTO in mealRequest.Dishes)
            {
                newMealDishes.Add(new MealDish
                {
                    Meal = newMeal,
                    Dish = new Dish
                    {
                        Id = dishDTO.Id,
                        Name = dishDTO.Name,
                        ImagePath = dishDTO.ImagePath,
                        UserId = userId,
                        Description = dishDTO.Description,
                        ExposeToOtherUsers = dishDTO.ExposeToOtherUsers
                    }
                });
            }

            try
            {
                _databaseContext.MealDishes.AttachRange(newMealDishes);
                await _databaseContext.Meals.AddAsync(newMeal);
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Meal>(false, exception: ex);
            }

            return new DatabaseActionResult<Meal>(true, obj: newMeal);
        }
        private async Task<DatabaseActionResult<Meal>> UpdateMealDishes(Meal existingMeal, PutMealRequest mealRequest)
        {
            try
            {
                List<MealDish> existingMealDishes = await _databaseContext.MealDishes
                    .Where(md => md.MealId.Equals(existingMeal.Id)).ToListAsync();


                if (existingMealDishes.Count < mealRequest.Dishes.Count)
                {
                    return await AddMealDish(mealRequest, existingMealDishes, existingMeal);
                }
                else
                {
                    return await RemoveMealDish(mealRequest, existingMealDishes, existingMeal);
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Meal>(false, exception: ex);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Meal>(false, exception: ex);
            }
        }

        private async Task<DatabaseActionResult<Meal>> AddMealDish(PutMealRequest mealRequest, List<MealDish> existingMealDishes, Meal existingMeal)
        {
            var leftJoinResult = mealRequest.Dishes.GroupJoin(
                 existingMealDishes,
                dishDTO => dishDTO.MealDishId,
                mealDish => mealDish.Id,
                (dishDto, mealDishes) => new
                {
                    DishId = dishDto.Id,
                    MealDish = mealDishes.DefaultIfEmpty().Select(mealDish => mealDish).FirstOrDefault()
                });

            var notExistingMealDish = leftJoinResult.Where(x => x.MealDish is null).SingleOrDefault();

            if (notExistingMealDish != null)
            {
                var dishToAdd = mealRequest.Dishes.Where(d => d.Id.Equals(notExistingMealDish.DishId)).FirstOrDefault();
                if (dishToAdd is not null)
                {
                    await _databaseContext.MealDishes.AddAsync(new MealDish
                    {
                        DishId = dishToAdd.Id,
                        MealId = existingMeal.Id
                    });
                }
            }

            await _databaseContext.SaveChangesAsync();
            return new DatabaseActionResult<Meal>(true, obj: existingMeal);
        }

        private async Task<DatabaseActionResult<Meal>> RemoveMealDish(PutMealRequest mealRequest, List<MealDish> existingMealDishes, Meal existingMeal)
        {
            var rightJoinResult = existingMealDishes.GroupJoin(
                 mealRequest.Dishes,
                 dishDTO => dishDTO.Id,
                 mealDish => mealDish.MealDishId,
                 (dishDto, mealDishes) => new
                 {
                     DishId = dishDto.Id,
                     MealDish = mealDishes.DefaultIfEmpty().Select(mealDish => mealDish).FirstOrDefault()
                 });

            var mealDishNonExistsInRequest = rightJoinResult.Where(x => x.MealDish is null).SingleOrDefault();
            var mealDishToRemove = _databaseContext.MealDishes.Where(d => d.Id.Equals(mealDishNonExistsInRequest.DishId)).FirstOrDefault();

            var customizedPortionToRemove = _databaseContext.CustomizedDishProducts.Where(c => c.MealDishId == mealDishToRemove.Id);

            _databaseContext.MealDishes.Remove(mealDishToRemove);
            _databaseContext.CustomizedDishProducts.RemoveRange(customizedPortionToRemove);

            await _databaseContext.SaveChangesAsync();
            return new DatabaseActionResult<Meal>(true, obj: existingMeal);
        }

        private async Task<DatabaseActionResult<Meal>> RemoveMealEntry(PutMealRequest mealRequest, DateTime date)
        {
            var existingMeal = await _databaseContext.Meals
                .Where(meal => meal.Date.Date == date.Date && meal.MealType == (int)mealRequest.MealType)
                .FirstOrDefaultAsync();

            var mealDishToRemove = await _databaseContext.MealDishes.Where(md => md.MealId == existingMeal.Id).SingleOrDefaultAsync();
            var customizedDPToRemove = await _databaseContext.CustomizedDishProducts.Where(c => c.MealDishId == mealDishToRemove.Id).SingleOrDefaultAsync();
            if (customizedDPToRemove != null)
            {
                _databaseContext.CustomizedDishProducts.Remove(customizedDPToRemove);
            }
            _databaseContext.Meals.Remove(existingMeal);

            await _databaseContext.SaveChangesAsync();

            return new DatabaseActionResult<Meal>(true, obj: null);
        }

    }
}
