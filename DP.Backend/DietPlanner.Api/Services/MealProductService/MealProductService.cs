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
        private readonly DietPlannerDbContext _databaseContext;
        private readonly ILogger<MealProductService> _logger;

        public MealProductService(DietPlannerDbContext databaseContext, ILogger<MealProductService> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task<DatabaseActionResult> UpdatePortionMultiplier(int dishId, int productId, decimal multiplier)
        {

            var dishProduct = await _databaseContext.DishProducts
                .Where(dp => dp.DishId == dishId && dp.ProductId == productId)
                .SingleAsync();

            dishProduct.PortionMultiplier = multiplier;

            _databaseContext.DishProducts.Update(dishProduct);
            await _databaseContext.SaveChangesAsync();

            return new DatabaseActionResult(true);
        }
    }
}