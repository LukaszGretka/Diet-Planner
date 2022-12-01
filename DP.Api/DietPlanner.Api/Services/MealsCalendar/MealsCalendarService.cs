using DietPlanner.Api.Database;
using DietPlanner.Api.Models;
using DietPlanner.Api.Models.Dto.MealsCalendar;
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

        public async Task<DailyMealsDTO> GetMeals(DateTime date)
        {
            string formattedDate = date.ToShortDateString();

            var productsIds = await _databaseContext.Meals.Join(
                _databaseContext.MealProducts,
                meals => meals.Id,
                mealProducts => mealProducts.MealId,
                (meal, mealProduct) => new 
                { 
                    mealId = meal.Id,
                    mealDate = meal.Date,
                    productId = mealProduct.ProductId,
                    mealType = meal.MealType
                })
                .Where(joinResult => joinResult.mealDate.Equals(formattedDate))
                .Select(x => x.productId)
                .ToListAsync();

            var products = await _databaseContext.Products.Where(product =>
                productsIds.Contains(product.Id)).ToListAsync();

            return new DailyMealsDTO
            {
                Breakfast = new MealDTO
                { 
                    MealType = MealTypeEnum.Breakfast, 
                    Products = new Product[] { }
                }
            };
        }

        public async Task<DatabaseActionResult<Meal>> AddMeal(MealByDay mealByDay)
        {
            var products = mealByDay.Products.ToList();
            var mealProducts = new List<MealProduct>();

            foreach (var product in products)
            {
                mealProducts.Add(new MealProduct
                {
                    ProductId = product.Id
                });
            }

            var meal = new Meal
            {
                Date = mealByDay.Date.ToShortDateString(),
                MealType = new MealType 
                { 
                    Name = mealByDay.MealType.ToString() 
                },
            };

            try
            {
                await _databaseContext.Meals.AddAsync(meal);
                await _databaseContext.MealProducts.AddRangeAsync(mealProducts);
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
