using DietPlanner.Api.Database;
using DietPlanner.Api.Extensions;
using DietPlanner.Api.Models;
using DietPlanner.Api.Models.MealsCalendar;
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

        public async Task<List<MealDTO>> GetMeals(DateTime date)
        {
            string formattedDate = date.Normalize();

            return await _databaseContext.Meals
                .Join(
                    _databaseContext.MealProducts,
                    meals => meals.Id,
                    mealProducts => mealProducts.Meal.Id,
                    (meal, mealProduct) => new
                    {
                        mealDate = meal.Date,
                        productId = mealProduct.Product.Id,
                        mealTypeId = meal.MealTypeId
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
                    })
                .Where(finalJoinResult => finalJoinResult.mealDate.Equals(formattedDate))
                .GroupBy(x => x.mealTypeId, (mealTypeId, product) => new MealDTO
                {
                    Products = product.Select(x => x.product).ToList(),
                    MealTypeId = (MealTypeEnum)mealTypeId
                })
                .ToListAsync();
        }

        public async Task<DatabaseActionResult<Meal>> AddOrUpdateMeal(MealByDay mealByDay)
        {
            var existingMeal = _databaseContext.Meals.AsNoTracking().SingleOrDefault(x =>
                x.Date.Equals(mealByDay.Date.Normalize())
                    && x.MealTypeId == (int)mealByDay.MealTypeId);

            if (existingMeal is not null)
            {
                return await UpdateMeal(existingMeal, mealByDay);
            }

            Meal newMeal = new()
            {
                Date = mealByDay.Date.Normalize(),
                MealTypeId = (int)mealByDay.MealTypeId
            };

            List<MealProduct> newMealProducts = await AddProductsToMeal(newMeal, mealByDay);

            try
            {
                _databaseContext.MealProducts.AttachRange(newMealProducts);
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

        private async Task<DatabaseActionResult<Meal>> UpdateMeal(Meal existingMeal, MealByDay mealByDay)
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
                    _databaseContext.Meals.Update(existingMeal);
                }
                else
                {
                    _databaseContext.Meals.Remove(existingMeal);
                }

                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Meal>(false, exception: ex);
            }

            return new DatabaseActionResult<Meal>(true, obj: existingMeal);
        }

        private async Task<List<MealProduct>> AddProductsToMeal(Meal meal, MealByDay mealByDay)
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
