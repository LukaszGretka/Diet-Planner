using DietPlanner.Api.Database;
using DietPlanner.Api.Database.Models;
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
    public class MealService//: IMealService
    {
        private readonly ILogger<MealService> _logger;
        private readonly DietPlannerDbContext _databaseContext;

        public MealService(ILogger<MealService> logger, DietPlannerDbContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        //public async Task<List<MealDto>> GetMeals(DateTime date, string userId)
        //{
        //    string formattedDate = date.ToDatabaseDateFormat();

        //    return await _databaseContext.Meals
        //        .Join(
        //            _databaseContext.MealDishes,
        //            meals => meals.Id,
        //            mealProducts => mealProducts.Meal.Id,
        //            (meal, mealDish) => new MealDto
        //            {
        //                MealTypeId = (MealTypeEnum)meal.MealTypeId,
        //                DishProducts = mealDish.Dishes.DishProducts.ToList(),
        //            })
        //        .Join(
        //            _databaseContext.Dishes,
        //            joinResult => joinResult.productId,
        //            product => product.Id,
        //            (joinResult, product) => new
        //            {
        //                joinResult.mealDate,
        //                joinResult.mealTypeId,
        //                product,
        //                joinResult.productPortionMultiplier,
        //                joinResult.userId
        //            })
        //        .Where(finalJoinResult => finalJoinResult.mealDate.Equals(formattedDate)
        //                && finalJoinResult.userId.Equals(userId)
        //        )
        //        //.GroupBy(x => x.mealTypeId, (mealTypeId, product) => new MealDto
        //        //{
        //        //    PortionProducts = product.Select(p => new ProductPortion
        //        //    {
        //        //        Id = p.product.Id,
        //        //        Name = p.product.Name,
        //        //        Description = p.product.Description,
        //        //        BarCode = p.product.BarCode,
        //        //        ImagePath = p.product.ImagePath,
        //        //        Calories = p.product.Calories * (float) p.productPortionMultiplier,
        //        //        Carbohydrates = p.product.Carbohydrates * (float)p.productPortionMultiplier,
        //        //        Proteins = p.product.Proteins * (float)p.productPortionMultiplier,
        //        //        Fats = p.product.Fats * (float)p.productPortionMultiplier,
        //        //        PortionMultiplier = p.productPortionMultiplier
        //        //    }).ToList(),
        //        //    MealTypeId = (MealTypeEnum)mealTypeId
        //        //})
        //        .ToListAsync();
        //}

        //public async Task<DatabaseActionResult<Meal>> AddOrUpdateMeal(MealByDay mealByDay, string userId)
        //{
        //    string formattedDate = mealByDay.Date.ToDatabaseDateFormat();

        //    Meal existingMeal = _databaseContext.Meals.AsNoTracking().SingleOrDefault(meal =>
        //        meal.Date.Equals(formattedDate)
        //        && meal.MealTypeId == (int)mealByDay.MealTypeId
        //        && meal.UserId.Equals(userId));

        //    if (existingMeal is not null)
        //    {
        //        return await UpdateMeal(existingMeal, mealByDay);
        //    }

        //    Meal newMeal = new()
        //    {
        //        Date = formattedDate,
        //        MealTypeId = (int)mealByDay.MealTypeId,
        //        UserId = userId
        //    };

        //    List<MealProduct> newMealProducts = await AddProductsToMeal(newMeal, mealByDay);

        //    try
        //    {
        //        _databaseContext.Meals.AttachRange(newMealProducts);
        //        await _databaseContext.Meals.AddAsync(newMeal);
        //        await _databaseContext.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return new DatabaseActionResult<Meal>(false, exception: ex);
        //    }

        //    return new DatabaseActionResult<Meal>(true, obj: newMeal);
        //}

        //private async Task<DatabaseActionResult<Meal>> UpdateMeal(Meal existingMeal, MealByDay mealByDay)
        //{
        //    List<MealProduct> newMealProducts = await AddProductsToMeal(existingMeal, mealByDay);

        //    try
        //    {
        //        var currentProducts = await _databaseContext.Meals
        //            .Where(mp => mp.Meal.Id == existingMeal.Id).ToListAsync();

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
