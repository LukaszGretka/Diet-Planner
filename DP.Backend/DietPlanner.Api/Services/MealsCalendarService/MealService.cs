using DietPlanner.Api.Database;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.Database.Repository;
using DietPlanner.Api.DTO;
using DietPlanner.Api.Models.MealProductModel;
using DietPlanner.Api.Models.MealsCalendar.DbModel;
using DietPlanner.Api.Models.MealsCalendar.DTO;
using DietPlanner.Api.Models.MealsCalendar.Requests;
using DietPlanner.Api.Services.Core;
using DietPlanner.Api.Services.MealProductService;
using DietPlanner.Domain.Enums;
using DietPlanner.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealsCalendar
{
    public class MealService(ILogger<MealService> logger, DietPlannerDbContext databaseContext, 
        IMealCalendarRepository repository, IRedisCacheService redisCacheService) : IMealService
    {
        private readonly ILogger<MealService> _logger = logger;
        private readonly DietPlannerDbContext _databaseContext = databaseContext;
        private readonly IMealCalendarRepository _repository = repository;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;

        public async Task<DatabaseActionResult<List<MealDto>>> GetMeals(DateTime date, string userId, CancellationToken ct)
        {
            string cacheKey = $"{nameof(MealService)}-{userId}-{date.ToShortDateString()}";
            string cachedMeals = await _redisCacheService.GetAsync(cacheKey, ct);

            if (!string.IsNullOrEmpty(cachedMeals))
            {
                return new DatabaseActionResult<List<MealDto>>(true, obj: JsonSerializer.Deserialize<List<MealDto>>(cachedMeals));
            }

            List<Meal> meals = await _databaseContext.Meals
                .Where(m => m.UserId == userId && m.Date.Date == date.Date)
                .ToListAsync(cancellationToken: ct);

            List<MealDto> mealDtos = [.. meals
                .GroupBy(m => new { m.MealType })
                .Select(g => new MealDto
                {
                    MealType = (MealType)g.Key.MealType,
                    Products = [.. g.SelectMany(meal => _repository.GetMealProducts(meal, ct).Result)],
                    Dishes = [.. g.SelectMany(meal => _repository.GetMealDishes(meal, ct).Result)]
                })];

            await _redisCacheService.SetAsync(cacheKey, mealDtos, ct);

            return new DatabaseActionResult<List<MealDto>>(true, obj: mealDtos);
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

            if(!result.Success)
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

            if(!result.Success)
            {
                return result;
            }

            await _redisCacheService.RemoveAsync(cacheKey, ct);
            return await GetMeals(request.Date, userId, ct);
        }

        private async Task<DatabaseActionResult<List<MealDto>>> UpdateDishPortion(UpdateMealItemPortionRequest request, CancellationToken ct)
        {
            CustomizedMealDishes customizedMealDishes = await _databaseContext.CustomizedMealDishes
                .Where(cdp => cdp.MealDishId == request.ItemProductId && cdp.DishProductId == request.DishProductId)
                .SingleOrDefaultAsync(ct);

            if (customizedMealDishes is null)
            {
                CustomizedMealDishes newCustomizedMealDishes = new CustomizedMealDishes
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
                customizedMealDishes.CustomizedPortionMultiplier = request.CustomizedPortionMultiplier;
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

            MealProduct mealProduct = new MealProduct {
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

        private async Task<DatabaseActionResult> RemoveDishFromMeal(int mealId,int mealItemId, CancellationToken ct)
        {
            MealDish mealDish = await _databaseContext.MealDishes
                .Where(md => md.Id == mealItemId && md.MealId == mealId).SingleOrDefaultAsync(ct);

            if (mealDish is null)
            {
                return new DatabaseActionResult(false, message: $"Meal item with Id {mealItemId} can't be found in meal");
            }

            List<CustomizedMealDishes> customizedMealDishes = await _databaseContext.CustomizedMealDishes
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
