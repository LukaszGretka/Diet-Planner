using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Application.Interfaces.Services;
using DietPlanner.Application.Models.MealsCalendar;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Enums;
using DietPlanner.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DietPlanner.Application.Services
{
    public class MealService(
        ILogger<MealService> logger,
        IMealRepository mealRepository,
        IMealProductRepository mealProductRepository,
        IMealDishRepository mealDishRepository,
        ICustomizedMealDishRepository customizedMealDishRepository,
        ICustomizedMealProductRepository customizedMealProductRepository,
        IMealCalendarRepository mealCalendarRepository,
        IRedisCacheService redisCacheService) : IMealService
    {
        private readonly ILogger<MealService> _logger = logger;
        private readonly IMealRepository _mealRepository = mealRepository;
        private readonly IMealProductRepository _mealProductRepository = mealProductRepository;
        private readonly IMealDishRepository _mealDishRepository = mealDishRepository;
        private readonly ICustomizedMealDishRepository _customizedMealDishRepository = customizedMealDishRepository;
        private readonly ICustomizedMealProductRepository _customizedMealProductRepository = customizedMealProductRepository;
        private readonly IMealCalendarRepository _mealCalendarRepository = mealCalendarRepository;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;

        public async Task<DatabaseActionResult<List<MealDto>>> GetMeals(DateTime date, string userId, CancellationToken ct)
        {
            string cacheKey = $"{nameof(MealService)}-{userId}-{date.ToShortDateString()}";
            string cachedMeals = await _redisCacheService.GetAsync(cacheKey, ct);

            if (!string.IsNullOrEmpty(cachedMeals))
            {
                return new DatabaseActionResult<List<MealDto>>(true, obj: JsonSerializer.Deserialize<List<MealDto>>(cachedMeals));
            }

            List<Meal> meals = await _mealRepository.GetMealsByDateAsync(date, userId, ct);

            List<MealDto> mealDtos = [.. meals
                .GroupBy(m => new { m.MealType })
                .Select(g => new MealDto
                {
                    MealType = (MealType)g.Key.MealType,
                    Products = [.. g.SelectMany(meal => _mealCalendarRepository.GetMealProducts(meal, ct).Result)],
                    Dishes = [.. g.SelectMany(meal => _mealCalendarRepository.GetMealDishes(meal, ct).Result)]
                })];

            await _redisCacheService.SetAsync(cacheKey, JsonSerializer.Serialize(mealDtos), ct);

            return new DatabaseActionResult<List<MealDto>>(true, obj: mealDtos);
        }

        public async Task<DatabaseActionResult<List<MealDto>>> AddMealItem(MealItemRequest addMealItemRequest, string userId, CancellationToken ct)
        {
            string cacheKey = $"{nameof(MealService)}-{userId}-{addMealItemRequest.Date.ToShortDateString()}";

            Meal foundMeal = await _mealRepository.GetMealByDateAndTypeAsync(
                addMealItemRequest.Date,
                (int)addMealItemRequest.MealType,
                userId,
                ct);

            foundMeal ??= new Meal
            {
                Date = addMealItemRequest.Date,
                MealType = (int)addMealItemRequest.MealType,
                UserId = userId,
            };

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

            Meal foundMeal = await _mealRepository.GetMealByDateAndTypeAsync(
                removeMealItemRequest.Date,
                (int)removeMealItemRequest.MealType,
                userId,
                ct);

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

            bool hasMealDishes = _mealDishRepository.GetQuery().Where(md => md.MealId == foundMeal.Id).Any();
            bool hasMealProducts = _mealProductRepository.GetQuery().Where(mp => mp.MealId == foundMeal.Id).Any();

            if (!hasMealDishes && !hasMealProducts)
            {
                await _mealRepository.RemoveMealAsync(foundMeal.Id, ct);
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
            var customizedMealDish = _customizedMealDishRepository.GetQuery()
                .Where(cdp => cdp.MealDishId == request.ItemProductId && cdp.DishProductId == request.DishProductId)
                .ToList()
                .FirstOrDefault();

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

                await _customizedMealDishRepository.CreateAsync(newCustomizedMealDishes, ct);
            }
            else
            {
                customizedMealDish.CustomizedPortionMultiplier = request.CustomizedPortionMultiplier;
                await _customizedMealDishRepository.UpdateAsync(customizedMealDish, ct);
            }

            return new DatabaseActionResult<List<MealDto>>(true);
        }

        private async Task<DatabaseActionResult<List<MealDto>>> UpdateProductPortion(UpdateMealItemPortionRequest request, CancellationToken ct)
        {
            var customizedMealProduct = _customizedMealProductRepository.GetQuery()
                .Where(cmp => cmp.MealProductId == request.ItemProductId)
                .ToList()
                .FirstOrDefault();

            if (customizedMealProduct is null)
            {
                CustomizedMealProducts newCustomizedMealProduct = new()
                {
                    MealProductId = request.ItemProductId,
                    CustomizedPortionMultiplier = request.CustomizedPortionMultiplier,
                };

                await _customizedMealProductRepository.CreateAsync(newCustomizedMealProduct, ct);
            }
            else
            {
                customizedMealProduct.CustomizedPortionMultiplier = request.CustomizedPortionMultiplier;
                await _customizedMealProductRepository.UpdateAsync(customizedMealProduct, ct);
            }

            return new DatabaseActionResult<List<MealDto>>(true);
        }

        private async Task<DatabaseActionResult> AddProductToMeal(Meal meal, int itemId, CancellationToken ct)
        {
            MealProduct mealProduct = new MealProduct
            {
                ProductId = itemId,
                Meal = meal,
                MealId = meal.Id
            };

            var result = await _mealProductRepository.CreateAsync(mealProduct, ct);

            return new DatabaseActionResult(true);
        }

        private async Task<DatabaseActionResult> AddDishToMeal(Meal meal, int itemId, CancellationToken ct)
        {
            MealDish mealDish = new MealDish
            {
                Meal = meal,
                MealId = meal.Id,
                DishId = itemId
            };

            await _mealDishRepository.CreateAsync(mealDish, ct);

            return new DatabaseActionResult(true);
        }

        private async Task<DatabaseActionResult> RemoveProductFromMeal(int mealId, int itemId, CancellationToken ct)
        {
            var mealProduct = _mealProductRepository.GetQuery()
                .Where(mp => mp.Id == itemId && mp.MealId == mealId)
                .ToList()
                .FirstOrDefault();

            if (mealProduct is null)
            {
                return new DatabaseActionResult(false, message: $"Product with id {itemId} can't be found in meal");
            }

            await _mealProductRepository.DeleteAsync(mealProduct, ct);

            List<CustomizedMealProducts> customizedMealProducts = _customizedMealProductRepository.GetQuery()
                .Where(cmp => cmp.MealProductId == itemId).ToList();

            if (customizedMealProducts.Count != 0)
            {
                foreach (var customizedProduct in customizedMealProducts)
                {
                    await _customizedMealProductRepository.DeleteAsync(customizedProduct, ct);
                }
            }

            return new DatabaseActionResult(true);
        }

        private async Task<DatabaseActionResult> RemoveDishFromMeal(int mealId, int mealItemId, CancellationToken ct)
        {
            var mealDish = _mealDishRepository.GetQuery()
                .Where(md => md.Id == mealItemId && md.MealId == mealId)
                .ToList()
                .FirstOrDefault();

            if (mealDish is null)
            {
                return new DatabaseActionResult(false, message: $"Meal item with Id {mealItemId} can't be found in meal");
            }

            List<CustomizedMealDish> customizedMealDishes = _customizedMealDishRepository.GetQuery()
                .Where(cmd => cmd.MealDishId == mealItemId).ToList();

            if (customizedMealDishes.Count != 0)
            {
                foreach (var customizedDish in customizedMealDishes)
                {
                    await _customizedMealDishRepository.DeleteAsync(customizedDish, ct);
                }
            }

            await _mealDishRepository.DeleteAsync(mealDish, ct);

            return new DatabaseActionResult(true);
        }
    }
}
