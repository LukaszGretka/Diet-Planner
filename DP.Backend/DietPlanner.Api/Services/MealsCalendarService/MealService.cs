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
                              .GroupBy(x => new { x.md.DishId, x.d.Name, x.d.Description, x.d.ImagePath, x.d.ExposeToOtherUsers, x.m.Id })
                              .Select(gd => new DishDTO
                              {
                                  Id = gd.Key.DishId,
                                  Name = gd.Key.Name,
                                  Description = gd.Key.Description,
                                  ImagePath = gd.Key.ImagePath,
                                  ExposeToOtherUsers = gd.Key.ExposeToOtherUsers,
                                  Products = gd.Join(_databaseContext.DishProducts, x => x.md.DishId, dp => dp.DishId, (x, dp) => new { x.m, x.md, dp })
                                               .Join(_databaseContext.Products, x => x.dp.ProductId, p => p.Id, (x, p) => new DishProductsDTO
                                               {
                                                   Product = p,
                                                   PortionMultiplier = x.dp.PortionMultiplier
                                               })
                                               .ToList()
                              })
                              .ToList()
                })
                .ToList();

            return mealDtos;
        }

        public async Task<DatabaseActionResult<Meal>> AddOrUpdateMeal(PutMealRequest mealRequest, string userId)
        {
            string formattedDate = mealRequest.Date.ToDatabaseDateFormat();

            //Meal existingMeal = _databaseContext.Meals.AsNoTracking().SingleOrDefault(meal =>
            //    meal.Date.Equals(formattedDate)
            //    && meal.MealType == (int)mealRequest.MealTypeId
            //    && meal.UserId.Equals(userId));

            //if (existingMeal is not null)
            //{
            //    return await UpdateMeal(existingMeal, mealRequest);
            //}

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

        //private async Task<DatabaseActionResult<Meal>> UpdateMeal(Meal existingMeal, PutMealRequest mealRequest)
        //{
        //    //update dish product table

        //    List<MealProduct> newMealProducts = await AddProductsToMeal(existingMeal, mealByDay);

        //    try
        //    {
        //        var currentMealDishes = await _databaseContext.DishProducts
        //            .Where(dishProduct => dishProduct.DishId == existingMeal.Id).ToListAsync();

        //        _databaseContext.Meals.RemoveRange(currentProducts);
        //        _databaseContext.Meals.AttachRange(newMealProducts);

        //        if (newMealProducts.Count > 0)
        //        {
        //            _databaseContext.Meals.Update(existingMeal);
        //        }
        //        else
        //        {
        //            _databaseContext.Meals.Remove(existingMeal);
        //        }

        //        await _databaseContext.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return new DatabaseActionResult<Meal>(false, exception: ex);
        //    }

        //    return new DatabaseActionResult<Meal>(true, obj: existingMeal);
        //}

        //private async Task<List<MealProduct>> AddProductsToMeal(Meal meal, MealByDay mealByDay)
        //{
        //    var mealProducts = new List<MealProduct>();
        //    var portionProducts = mealByDay.PortionProducts;

        //    foreach (var product in portionProducts)
        //    {
        //        var existingProduct = await _databaseContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        //        mealProducts.Add(new MealProduct
        //        {
        //            Product = existingProduct,
        //            Meal = meal,
        //            PortionMultiplier = product.PortionMultiplier
        //        });
        //    }

        //    return mealProducts;
        //}
    }
}
