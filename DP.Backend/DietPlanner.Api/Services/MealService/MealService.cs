using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.DTO.Products;
using DietPlanner.Api.Models.MealProductModel;
using DietPlanner.Api.Models.MealsCalendar.DTO;
using DietPlanner.Api.Models.MealsCalendar.Requests;
using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Interfaces.Repository;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Enums;
using DietPlanner.Infrastructure.Database;
using DietPlanner.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealService
{
    public class MealService(ILogger<MealService> logger, DietPlannerDbContext databaseContext,
        IMealDishRepository mealDishRepository, IMealProductRepository mealProductRepository,
        IRedisCacheService redisCacheService, IDishProductRepository dishProductRepository, 
        ICustomizedMealProductRepository customizedMealProductRepository) : IMealService
    {
        private readonly ILogger<MealService> _logger = logger;
        private readonly DietPlannerDbContext _databaseContext = databaseContext;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;

        public async Task<DatabaseActionResult<List<MealDto>>> GetMeals(DateTime date, string userId, CancellationToken ct)
        {
            return new DatabaseActionResult<List<MealDto>>(false, "Not implemented yet");
            //string cacheKey = $"{nameof(MealService)}-{userId}-{date.ToShortDateString()}";
            //string cachedMeals = await _redisCacheService.GetAsync(cacheKey, ct);

            //if (!string.IsNullOrEmpty(cachedMeals))
            //{
            //    return new DatabaseActionResult<List<MealDto>>(true, obj: JsonSerializer.Deserialize<List<MealDto>>(cachedMeals));
            //}

            //List<Meal> meals = await _databaseContext.Meals
            //    .Where(m => m.UserId == userId && m.Date.Date == date.Date)
            //    .ToListAsync(cancellationToken: ct);

            //List<MealDto> mealDtos = [.. meals
            //    .GroupBy(m => new { m.MealType })
            //    .Select(g => new MealDto
            //    {
            //        MealType = (MealType)g.Key.MealType,
            //        Products = [.. g.SelectMany(meal => mealProductRepository.GetMealProducts(meal.Id, ct).Result
            //        .Select(mp => new ProductDTO
            //        {
            //            MealItemId = mp.Id,
            //            Id = mp.Product.Id,
            //            Name = mp.Product.Name,
            //            Description = mp.Product.Description,
            //            ImagePath = mp.Product.ImagePath,
            //            ItemType = ItemType.Product,
            //            BarCode = mp.Product.BarCode,
            //            Calories = (float)mp.Product.Calories,
            //            Carbohydrates = (float)mp.Product.Carbohydrates,
            //            Proteins = (float)mp.Product.Proteins,
            //            Fats = (float)mp.Product.Fats,
            //            PortionMultiplier = customizedMealProductRepository.GetPortionMultiplierAsync(mp.Id, ct).Result
            //        })
            //        .Where(r => r.PortionMultiplier is null or 0m)
            //        .ToList()
            //        .ForEach(r => r.PortionMultiplier = 1.0m))
            //        ],

            //        Dishes = [.. g.SelectMany(meal => mealDishRepository.GetAllAsync(meal.Id, ct).Result)
            //        .Select(md => new DishDTO
            //        {
            //            Id = md.Dish.Id,
            //            MealItemId = md.Id,
            //            Description = md.Dish.Description,
            //            ExposeToOtherUsers = md.Dish.ExposeToOtherUsers,
            //            ImagePath = md.Dish.ImagePath,
            //            IsOwner = md.Dish.UserId == userId,
            //            ItemType = ItemType.Dish,
            //            Name = md.Dish.Name,
            //        Products = [.. dishProducts
            //        .Where(dp => dp.DishId == md.DishId)
            //        .Select(dp => new DishProductsDTO
            //        {
            //            DishProductId= dp.Id,
            //            Product = new Product
            //            {
            //                Id = dp.Product.Id,
            //                Name = dp.Product.Name,
            //                Description = dp.Product.Description,
            //                ImagePath = dp.Product.ImagePath,
            //                BarCode = dp.Product.BarCode,
            //                Calories = (float)dp.Product.Calories,
            //                Carbohydrates = (float)dp.Product.Carbohydrates,
            //                Proteins = (float)dp.Product.Proteins,
            //                Fats = (float)dp.Product.Fats,
            //            },
            //            PortionMultiplier = dp.PortionMultiplier,
            //            CustomizedPortionMultiplier = customizedMealDishes
            //                .Where(cdp => cdp.MealDishId == md.Id && cdp.DishProductId == dp.Id)
            //                .Select(cmd => cmd.CustomizedPortionMultiplier)
            //                .SingleOrDefault()
            //        })]})]})];

            //await _redisCacheService.SetAsync(cacheKey, mealDtos, ct);

            //return new DatabaseActionResult<List<MealDto>>(true, obj: mealDtos);
        }

        public async Task<DatabaseActionResult<List<MealDto>>> AddMealItem(MealItemRequest addMealItemRequest, string userId, CancellationToken ct)
        {
            string cacheKey = $"{nameof(MealService)}-{userId}-{addMealItemRequest.Date.ToShortDateString()}";

            Meal foundMeal = await _databaseContext.Meals
                .Where(meal => meal.Date.Date == addMealItemRequest.Date.Date
                    && meal.MealType == (int)addMealItemRequest.MealType
                    && meal.UserId == userId)
                .SingleOrDefaultAsync(ct);

            foundMeal ??= CreateNewMeal(addMealItemRequest.Date, addMealItemRequest.MealType, userId);

            var result = addMealItemRequest.ItemType switch
            {
                ItemType.Dish => await AddDishToMeal(foundMeal, addMealItemRequest.ItemId, ct),
                ItemType.Product => await AddProductToMeal(foundMeal, addMealItemRequest.ItemId, ct),
                _ => new DatabaseActionResult(false, "Provided invalid item type")
            };

            if (!result.Success)
            {
                return new DatabaseActionResult<List<MealDto>>(false, result.Message);
            }

            await _redisCacheService.RemoveAsync(cacheKey, ct);

            return await GetMeals(addMealItemRequest.Date, userId, ct);
        }

        public async Task<DatabaseActionResult<List<MealDto>>> RemoveMealItem(MealItemRequest removeMealItemRequest, string userId, CancellationToken ct)
        {
            string cacheKey = $"{nameof(MealService)}-{userId}-{removeMealItemRequest.Date.ToShortDateString()}";

            Meal foundMeal = await _databaseContext.Meals
                .Where(meal => meal.Date.Date == removeMealItemRequest.Date.Date && meal.MealType == (int)removeMealItemRequest.MealType)
                .SingleOrDefaultAsync(ct);

            if (foundMeal is null)
            {
                return new DatabaseActionResult<List<MealDto>>(false, "No meal found for provided parameters");
            }

            var result = removeMealItemRequest.ItemType switch
            {
                ItemType.Dish => await RemoveDishFromMeal(foundMeal.Id, removeMealItemRequest.MealItemId, ct),
                ItemType.Product => await RemoveProductFromMeal(foundMeal.Id, removeMealItemRequest.MealItemId, ct),
                _ => new DatabaseActionResult(false, "Provided invalid item type")
            };

            if (!result.Success)
            {
                return new DatabaseActionResult<List<MealDto>>(false, result.Message);
            }

            try
            {
                await _databaseContext.SaveChangesAsync(ct);

                bool hasMealDishes = await _databaseContext.MealDishes.AnyAsync(md => md.MealId == foundMeal.Id, ct);
                bool hasMealProducts = await _databaseContext.MealProducts.AnyAsync(mp => mp.MealId == foundMeal.Id, ct);

                if (!hasMealDishes && !hasMealProducts)
                {
                    _databaseContext.Meals.Remove(foundMeal);
                    await _databaseContext.SaveChangesAsync(ct);
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<List<MealDto>>(false, exception: ex);
            }

            await _redisCacheService.RemoveAsync(cacheKey, ct);

            return await GetMeals(removeMealItemRequest.Date, userId, ct);
        }

        public async Task<DatabaseActionResult<List<MealDto>>> UpdateMealItemPortion(UpdateMealItemPortionRequest request, string userId, CancellationToken ct)
        {
            string cacheKey = $"{nameof(MealService)}-{userId}-{request.Date.ToShortDateString()}";

            var result = request.ItemType switch
            {
                ItemType.Dish => await UpdateDishPortion(request, ct),
                ItemType.Product => await UpdateProductPortion(request, ct),
                _ => new DatabaseActionResult<List<MealDto>>(false, "Provided invalid item type")
            };

            if (!result.Success)
            {
                return result;
            }

            await _redisCacheService.RemoveAsync(cacheKey, ct);
            return await GetMeals(request.Date, userId, ct);
        }

        private async Task<DatabaseActionResult<List<MealDto>>> UpdateDishPortion(UpdateMealItemPortionRequest request, CancellationToken ct)
        {
            CustomizedMealDish customizedMealDish = await _databaseContext.CustomizedMealDishes
                .Where(cdp => cdp.MealDishId == request.ItemProductId && cdp.DishProductId == request.DishProductId)
                .SingleOrDefaultAsync(ct);

            if (customizedMealDish is null)
            {
                CustomizedMealDish newCustomizedMealDishes = new CustomizedMealDish
                {
                    DishProductId = request.DishProductId ??
                        throw new ArgumentNullException(nameof(request.DishProductId),
                        "DishProductId cannot be null. Please provide a valid value."),
                    CustomizedPortionMultiplier = request.CustomizedPortionMultiplier,
                    MealDishId = request.ItemProductId
                };

                await _databaseContext.CustomizedMealDishes.AddAsync(newCustomizedMealDishes, ct);
            }
            else
            {
                customizedMealDish.CustomizedPortionMultiplier = request.CustomizedPortionMultiplier;
            }

            try
            {
                await _databaseContext.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<List<MealDto>>(false, exception: ex);
            }

            return new DatabaseActionResult<List<MealDto>>(true);
        }

        private async Task<DatabaseActionResult<List<MealDto>>> UpdateProductPortion(UpdateMealItemPortionRequest request, CancellationToken ct)
        {
            CustomizedMealProducts customizedMealProduct = await _databaseContext.CustomizedMealProducts
             .Where(cmp => cmp.MealProductId == request.ItemProductId)
             .SingleOrDefaultAsync(ct);

            if (customizedMealProduct is null)
            {
                CustomizedMealProducts newCustomizedMealProduct = new()
                {
                    MealProductId = request.ItemProductId,
                    CustomizedPortionMultiplier = request.CustomizedPortionMultiplier,
                };

                await _databaseContext.CustomizedMealProducts.AddAsync(newCustomizedMealProduct, ct);
            }
            else
            {
                customizedMealProduct.CustomizedPortionMultiplier = request.CustomizedPortionMultiplier;
            }

            try
            {
                await _databaseContext.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<List<MealDto>>(false, exception: ex);
            }

            return new DatabaseActionResult<List<MealDto>>(true);
        }

        private async Task<DatabaseActionResult> AddProductToMeal(Meal meal, int itemId, CancellationToken ct)
        {
            Product product = await _databaseContext.Products.FindAsync(itemId, ct);

            if (product is null)
            {
                return new DatabaseActionResult(false, message: $"Product with id {itemId} can't be found");
            }

            MealProduct mealProduct = new MealProduct
            {
                ProductId = itemId,
                Meal = meal,
                Product = product,
                MealId = meal.Id
            };

            var result = await _databaseContext.MealProducts.AddAsync(mealProduct, ct);

            try
            {
                await _databaseContext.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult(false, exception: ex);
            }

            return new DatabaseActionResult(true);
        }

        private async Task<DatabaseActionResult> AddDishToMeal(Meal meal, int itemId, CancellationToken ct)
        {
            Dish dish = await _databaseContext.Dishes.FindAsync(itemId, ct);

            if (dish is null)
            {
                return new DatabaseActionResult(false, message: $"Dish with id {itemId} can't be found");
            }

            _databaseContext.MealDishes.Add(new MealDish
            {
                Meal = meal,
                Dish = dish
            });

            try
            {
                await _databaseContext.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult(false, exception: ex);
            }

            return new DatabaseActionResult(true);
        }

        private async Task<DatabaseActionResult> RemoveProductFromMeal(int mealId, int itemId, CancellationToken ct)
        {
            MealProduct mealProduct = await _databaseContext.MealProducts
                .Where(mp => mp.Id == itemId && mp.MealId == mealId)
                .SingleOrDefaultAsync(ct);

            if (mealProduct is null)
            {
                return new DatabaseActionResult(false, message: $"Product with id {itemId} can't be found in meal");
            }

            _databaseContext.MealProducts.Remove(mealProduct);

            List<CustomizedMealProducts> customizedMealProducts = await _databaseContext.CustomizedMealProducts
                .Where(cmp => cmp.MealProductId == itemId).ToListAsync(ct);

            if (customizedMealProducts.Count != 0)
            {
                _databaseContext.CustomizedMealProducts.RemoveRange(customizedMealProducts);
            }

            return new DatabaseActionResult(true);
        }

        private async Task<DatabaseActionResult> RemoveDishFromMeal(int mealId, int mealItemId, CancellationToken ct)
        {
            MealDish mealDish = await _databaseContext.MealDishes
                .Where(md => md.Id == mealItemId && md.MealId == mealId).SingleOrDefaultAsync(ct);

            if (mealDish is null)
            {
                return new DatabaseActionResult(false, message: $"Meal item with Id {mealItemId} can't be found in meal");
            }

            List<CustomizedMealDish> customizedMealDishes = await _databaseContext.CustomizedMealDishes
                .Where(cmd => cmd.MealDishId == mealItemId).ToListAsync(ct);

            if (customizedMealDishes.Count != 0)
            {
                _databaseContext.CustomizedMealDishes.RemoveRange(customizedMealDishes);
            }

            _databaseContext.MealDishes.Remove(mealDish);

            return new DatabaseActionResult(true);
        }

        private static Meal CreateNewMeal(DateTime date, MealType mealType, string userId)
        {
            return new()
            {
                Date = date,
                MealType = (int)mealType,
                UserId = userId,
            };
        }
    }
}
