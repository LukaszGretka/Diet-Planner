using DietPlanner.Api.Database;
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
            string formattedDate = date.ToShortDateString();

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

        public async Task<DatabaseActionResult<Meal>> AddMeal(MealByDay mealByDay)
        {
            var products = mealByDay.Products;
            var mealProducts = new List<MealProduct>();

            var meal = new Meal
            {
                Date = mealByDay.Date.ToShortDateString(),
                MealTypeId = (int)mealByDay.MealTypeId
            };

            products.ForEach(product => mealProducts.Add(
                new MealProduct
                {
                    Product = product,
                    Meal = meal
                })
            );

            try
            {
                _databaseContext.MealProducts.AttachRange(mealProducts);
                await _databaseContext.Meals.AddAsync(meal);
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Meal>(false, exception: ex);
            }

            return new DatabaseActionResult<Meal>(true, obj: meal);
        }
    }
}
