using DietPlanner.Api.Database;
using DietPlanner.Api.Extensions;
using DietPlanner.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealProductService
{
    public class MealProductService : IMealProductService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<MealProductService> _logger;

        public MealProductService(DatabaseContext databaseContext, ILogger<MealProductService> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task<DatabaseActionResult> UpdatePortionMultiplier(DateTime date, Enums.MealType mealType, int productId, decimal multiplier)
        {
            string formattedDate = date.ToDatabaseDateFormat();

            var mealId = await _databaseContext.UserMeals
                .Where(meal => meal.Date.Equals(formattedDate) && meal.MealTypeId == (int)mealType)
                .Select(meal => meal.Id)
                .FirstOrDefaultAsync();

            var mealProduct = await _databaseContext.MealProducts
                .Where(mp => mp.Product.Id == productId && mp.Meal.Id == mealId)
                .SingleOrDefaultAsync();

            if (mealProduct is null)
            {
                _logger.LogError($"Meal id: {mealId}, product id: {productId} no found");
                return new DatabaseActionResult(false, "Meal Product no found");
            }

            mealProduct.PortionMultiplier = multiplier;

            _databaseContext.MealProducts.Update(mealProduct);
            await _databaseContext.SaveChangesAsync();

            return new DatabaseActionResult(true);
        }
    }
}