using DietPlanner.Api.Database;
using DietPlanner.Api.Extensions;
using DietPlanner.Api.Models.MealsCalendar.DbModel;
using DietPlanner.Api.Models.MealsCalendar.DTO;
using DietPlanner.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealsCalendar
{
    public class MealsCalendarService : IMealsCalendarService
    {
        private readonly ILogger<MealsCalendarService> _logger;
        private readonly DatabaseContext _databaseContext;

        public MealsCalendarService(ILogger<MealsCalendarService> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<List<MealDto>> GetMeals(DateTime date, string userId)
        {
            string formattedDate = date.ToDatabaseDateFormat();

            return await _databaseContext.UserMeals
                .Join(
                    _databaseContext.MealProducts,
                    meals => meals.Id,
                    mealProducts => mealProducts.Meal.Id,
                    (meal, mealProduct) => new
                    {
                        mealDate = meal.Date,
                        productId = mealProduct.Product.Id,
                        mealTypeId = meal.MealTypeId,
                        userId = meal.UserId
                    })
                .Join(
                    _databaseContext.Products,
                    joinResult => joinResult.productId,
                    product => product.Id,
                    (joinResult, product) => new
                    {
                        joinResult.mealDate,
                        joinResult.mealTypeId,
                        product,
                        joinResult.userId
                    })
                .Where(finalJoinResult => finalJoinResult.mealDate.Equals(formattedDate) 
                        && finalJoinResult.userId.Equals(userId)
                )
                .GroupBy(x => x.mealTypeId, (mealTypeId, product) => new MealDto
                {
                    Products = product.Select(x => x.product).ToList(),
                    MealTypeId = (MealTypeEnum)mealTypeId
                })
                .ToListAsync();
        }

        public async Task<DatabaseActionResult<UserMeal>> AddOrUpdateMeal(MealByDay mealByDay, string userId)
        {
            string formattedDate = mealByDay.Date.ToDatabaseDateFormat();

            UserMeal existingMeal = _databaseContext.UserMeals.AsNoTracking().SingleOrDefault(meal =>
                meal.Date.Equals(formattedDate)
                && meal.MealTypeId == (int)mealByDay.MealTypeId
                && meal.UserId.Equals(userId));

            if (existingMeal is not null)
            {
                return await UpdateMeal(existingMeal, mealByDay);
            }

            UserMeal newMeal = new()
            {
                Date = formattedDate,
                MealTypeId = (int)mealByDay.MealTypeId,
                UserId = userId
            };

            List<MealProduct> newMealProducts = await AddProductsToMeal(newMeal, mealByDay);

            try
            {
                _databaseContext.MealProducts.AttachRange(newMealProducts);
                await _databaseContext.UserMeals.AddAsync(newMeal);
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<UserMeal>(false, exception: ex);
            }

            return new DatabaseActionResult<UserMeal>(true, obj: newMeal);
        }

        private async Task<DatabaseActionResult<UserMeal>> UpdateMeal(UserMeal existingMeal, MealByDay mealByDay)
        {
            List<MealProduct> newMealProducts = await AddProductsToMeal(existingMeal, mealByDay);

            try
            {
                var currentProducts = await _databaseContext.MealProducts
                    .Where(mp => mp.Meal.Id == existingMeal.Id).ToListAsync();

                _databaseContext.MealProducts.RemoveRange(currentProducts);
                _databaseContext.MealProducts.AttachRange(newMealProducts);

                if (newMealProducts.Count > 0)
                {
                    _databaseContext.UserMeals.Update(existingMeal);
                }
                else
                {
                    _databaseContext.UserMeals.Remove(existingMeal);
                }

                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<UserMeal>(false, exception: ex);
            }

            return new DatabaseActionResult<UserMeal>(true, obj: existingMeal);
        }

        private async Task<List<MealProduct>> AddProductsToMeal(UserMeal meal, MealByDay mealByDay)
        {
            var mealProducts = new List<MealProduct>();
            var products = mealByDay.Products;

            foreach (var product in products)
            {
                var existingProduct = await _databaseContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
                mealProducts.Add(new MealProduct
                {
                    Product = existingProduct,
                    Meal = meal
                });
            }

            return mealProducts;
        }
    }
}
