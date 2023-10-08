using DietPlanner.Api.Database;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.DTO.Dishes;
using DietPlanner.Api.Extensions;
using DietPlanner.Api.Models.MealsCalendar.DTO;
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

            var mealDtos = _databaseContext.Meals
                .Where(m => m.UserId == userId && m.Date == formattedDate)
                .GroupBy(m => new { m.MealType })
                .Select(g => new MealDto
                {
                    MealTypeId = (MealTypeEnum)g.Key.MealType,
                    Dishes = g.Join(_databaseContext.MealDishes, m => m.Id, md => md.MealId, (m, md) => new { m, md })
                              .Join(_databaseContext.Dishes, x => x.md.DishId, d => d.Id, (x, d) => new { x.m, x.md, d })
                              .GroupBy(x => new { x.md.DishId, x.d.Name, x.d.Description, x.d.ImagePath, x.d.ExposeToOtherUsers, x.m.Id, mealDishId = x.md.Id })
                              .Select(gd => new DishDTO
                              {
                                  Id = gd.Key.DishId,
                                  MealDishId = gd.Key.mealDishId,
                                  Name = gd.Key.Name,
                                  Description = gd.Key.Description,
                                  ImagePath = gd.Key.ImagePath,
                                  ExposeToOtherUsers = gd.Key.ExposeToOtherUsers,
                                  Products = gd.Join(_databaseContext.DishProducts, x => x.md.DishId, dp => dp.DishId, (x, dp) => new { x.m, x.md, dp })
                                               .Join(_databaseContext.Products, x => x.dp.ProductId, p => p.Id, (x, p) => new DishProductsDTO
                                               {
                                                   Product = p,
                                                   PortionMultiplier = x.dp.PortionMultiplier,
                                                   CustomizedPortionMultiplier = x.dp.CustomizedPortionMultiplier
                                               })
                              }) as List<DishDTO>
                });

            return await mealDtos.ToListAsync();
        }

        public async Task<DatabaseActionResult<Meal>> AddOrUpdateMeal(PutMealRequest mealRequest, string userId)
        {
            string formattedDate = mealRequest.Date.ToDatabaseDateFormat();

            var dishIds = mealRequest.Dishes.Select(dish => dish.Id).ToList();

            if (!dishIds.Any())
            {
                return await RemoveMealEntry(mealRequest, formattedDate);
            }

            var existingMeal = await _databaseContext.Meals
                .Where(meal => meal.Date.Equals(formattedDate)
                    && meal.MealType == (int)mealRequest.MealTypeId)
                .SingleOrDefaultAsync();


            if (existingMeal is not null)
            {
                return await UpdateMealDishes(existingMeal, mealRequest);
            }

            Meal newMeal = new()
            {
                Date = formattedDate,
                MealType = (int)mealRequest.MealTypeId,
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
            var dishToRemove = _databaseContext.MealDishes.Where(d => d.Id.Equals(mealDishNonExistsInRequest.DishId)).FirstOrDefault();

            _databaseContext.MealDishes.Remove(dishToRemove);

            await _databaseContext.SaveChangesAsync();
            return new DatabaseActionResult<Meal>(true, obj: existingMeal);
        }

        private async Task<DatabaseActionResult<Meal>> RemoveMealEntry(PutMealRequest mealRequest, string formattedDate)
        {
            var existingMeals = await _databaseContext.Meals
                .Where(meal => meal.Date.Equals(formattedDate) && meal.MealType == (int)mealRequest.MealTypeId)
                .FirstOrDefaultAsync();

            _databaseContext.Meals.Remove(existingMeals);

            await _databaseContext.SaveChangesAsync();

            return new DatabaseActionResult<Meal>(true, obj: null);
        }
    }
}
