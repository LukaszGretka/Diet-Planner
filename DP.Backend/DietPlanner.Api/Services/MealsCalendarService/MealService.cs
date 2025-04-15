using DietPlanner.Api.Database;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.Database.Repository;
using DietPlanner.Api.DTO;
using DietPlanner.Api.DTO.Products;
using DietPlanner.Api.Enums;
using DietPlanner.Api.Models.MealProductModel;
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
        private readonly IMealCalendarRepository _repository;

        public MealService(ILogger<MealService> logger, DietPlannerDbContext databaseContext, IMealCalendarRepository repository)
        {
            _logger = logger;
            _databaseContext = databaseContext;
            _repository = repository;
        }

        public async Task<List<MealDto>> GetMeals(DateTime date, string userId)
        {
            List<Meal> meals = await _databaseContext.Meals
                .Where(m => m.UserId == userId && m.Date.Date == date.Date)
                .ToListAsync();

            var mealDtos = meals
                .GroupBy(m => new { m.MealType })
                .Select(g => new MealDto
                {
                    MealType = (MealType)g.Key.MealType,
                    Products = g.SelectMany(meal => _repository.GetMealProducts(meal)).ToList(),
                    Dishes = g.SelectMany(meal => _repository.GetMealDishes(meal)).ToList()
                }).ToList();

            return mealDtos;
        }

        public async Task<DatabaseActionResult<Meal>> AddMealItem(MealItemRequest addMealItemRequest, string userId)
        {
            Meal foundMeal = await _databaseContext.Meals
                .Where(meal => meal.Date.Date == addMealItemRequest.Date.Date
                    && meal.MealType == (int)addMealItemRequest.MealType
                    && meal.UserId == userId)
                .SingleOrDefaultAsync();

            foundMeal ??= CreateNewMeal(addMealItemRequest.Date, addMealItemRequest.MealType, userId);

            return addMealItemRequest.ItemType switch
            {
                ItemType.Dish => await AddDishToMeal(foundMeal, addMealItemRequest.ItemId),
                ItemType.Product => await AddProductToMeal(foundMeal, addMealItemRequest.ItemId),
                _ => new DatabaseActionResult<Meal>(false, obj: null),
            };
        }

        public async Task<DatabaseActionResult<Meal>> RemoveMealItem(MealItemRequest removeMealItemRequest, string userId)
        {
            Meal foundMeal = await _databaseContext.Meals
                .Where(meal => meal.Date.Date == removeMealItemRequest.Date.Date && meal.MealType == (int)removeMealItemRequest.MealType)
                .SingleOrDefaultAsync();

            if (foundMeal is null)
            {
                return new DatabaseActionResult<Meal>(false, "No meal found for provided parameters");
            }

            return removeMealItemRequest.ItemType switch
            {
                ItemType.Dish => await RemoveDishFromMeal(foundMeal, removeMealItemRequest.MealItemId),
                ItemType.Product => await RemoveProductFromMeal(foundMeal, removeMealItemRequest.MealItemId),
                _ => new DatabaseActionResult<Meal>(false, obj: null),
            };
        }

        public async Task<DatabaseActionResult> UpdateMealItemPortion(UpdateMealItemPortionRequest request)
        {
            return request.ItemType switch
            {
                ItemType.Dish => await UpdateDishPortion(request),
                ItemType.Product => await UpdateProductPortion(request),
                _ => new DatabaseActionResult(false, message: "Invalid item type provided."),
            };
        }

        private async Task<DatabaseActionResult> UpdateDishPortion(UpdateMealItemPortionRequest request)
        {
            var customizedDishProduct = await _databaseContext.CustomizedMealDishes
                .Where(cdp => cdp.MealDishId == request.ItemProductId)
                .SingleOrDefaultAsync();

            if (customizedDishProduct is null)
            {
                var newCustomizedDishProduct = new CustomizedMealDishes
                {
                    DishProductId = request.DishProductId ??
                        throw new ArgumentNullException(nameof(request.DishProductId),
                        "DishProductId cannot be null. Please provide a valid value."),
                    CustomizedPortionMultiplier = request.CustomizedPortionMultiplier,
                    MealDishId = request.ItemProductId
                }
            ;

                await _databaseContext.CustomizedMealDishes.AddAsync(newCustomizedDishProduct);
            }
            else
            {
                customizedDishProduct.CustomizedPortionMultiplier = request.CustomizedPortionMultiplier;
            }

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult(false, exception: ex);
            }

            return new DatabaseActionResult(true);
        }

        private async Task<DatabaseActionResult> UpdateProductPortion(UpdateMealItemPortionRequest request)
        {
            CustomizedMealProducts customizedMealProduct = await _databaseContext.CustomizedMealProducts
             .Where(cmp => cmp.MealProductId == request.ItemProductId)
             .SingleOrDefaultAsync();

            if (customizedMealProduct is null)
            {
                CustomizedMealProducts newCustomizedMealProduct = new()
                {
                    MealProductId = request.ItemProductId,
                    CustomizedPortionMultiplier = request.CustomizedPortionMultiplier,
                };

                await _databaseContext.CustomizedMealProducts.AddAsync(newCustomizedMealProduct);
            }
            else
            {
                customizedMealProduct.CustomizedPortionMultiplier = request.CustomizedPortionMultiplier;
            }

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult(false, exception: ex);
            }

            return new DatabaseActionResult(true);
        }

        private async Task<DatabaseActionResult<Meal>> AddProductToMeal(Meal meal, int itemId)
        {
            Product product = _databaseContext.Products.Find(itemId);

            if (product is null)
            {
                return new DatabaseActionResult<Meal>(false, obj: null, message: $"Product with id {itemId} can't be found");
            }

            MealProduct mealProduct = new MealProduct {
                ProductId = itemId,
                Meal = meal,
                Product = product,
                MealId = meal.Id
            };


            _databaseContext.MealProducts.Add(mealProduct);

            try
            {
                await _databaseContext.SaveChangesAsync();
                //await AddDefaultCustomizedMealProduct(mealProduct.Id);
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
                UserId = userId,
            };
        }

        private async Task<DatabaseActionResult<Meal>> RemoveProductFromMeal(Meal meal, int itemId)
        {
            MealProduct mealProduct = await _databaseContext.MealProducts
                .Where(mp => mp.Id == itemId && mp.MealId == meal.Id)
                .SingleOrDefaultAsync();

            if (mealProduct is null)
            {
                return new DatabaseActionResult<Meal>(false, obj: null, message: $"Product with id {itemId} can't be found in meal");
            }

            _databaseContext.MealProducts.Remove(mealProduct);

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

        private async Task<DatabaseActionResult<Meal>> RemoveDishFromMeal(Meal meal, int mealItemId)
        {
            MealDish mealDish = await _databaseContext.MealDishes
                .Where(md => md.Id == mealItemId).SingleOrDefaultAsync();

            if (mealDish is null)
            {
                return new DatabaseActionResult<Meal>(false, obj: null, message: $"Meal item with Id {mealItemId} can't be found in meal");
            }

            List<CustomizedMealDishes> customizedMealDishes = await _databaseContext.CustomizedMealDishes
                .Where(cmd => cmd.MealDishId == mealItemId).ToListAsync();

            if (customizedMealDishes.Any()) {
                _databaseContext.CustomizedMealDishes.RemoveRange(customizedMealDishes);
            }
            _databaseContext.MealDishes.Remove(mealDish);

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

        private async Task<DatabaseActionResult> AddDefaultCustomizedMealProduct(int mealProductId)
        {
            _databaseContext.CustomizedMealProducts.Add(new CustomizedMealProducts
            {
                MealProductId = mealProductId,
                CustomizedPortionMultiplier = 1
            });

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult(false, exception: ex);
            }
            return new DatabaseActionResult(true);
        }
    }
}
