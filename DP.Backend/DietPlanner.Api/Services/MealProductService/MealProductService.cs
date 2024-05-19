using DietPlanner.Api.Database;
using DietPlanner.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services.MealProductService
{
    public class MealProductService : IMealProductService
    {
        private readonly DietPlannerDbContext _databaseContext;

        public MealProductService(DietPlannerDbContext databaseContext, ILogger<MealProductService> logger)
        {
            _databaseContext = databaseContext;
        }

        public async Task<DatabaseActionResult> AddOrUpdateCustomizedPortionMultiplier(int dishId, int productId, int mealDishId, decimal customizedMultiplier)
        {
                var dishProduct = await _databaseContext.DishProducts
              .Where(dp => dp.DishId == dishId && dp.ProductId == productId)
              .SingleOrDefaultAsync();

            if(dishProduct is null)
            {
                return new DatabaseActionResult(false, "dish product not found");
            }

            var customizedDishProduct = await _databaseContext.CustomizedDishProducts
                .Where(cdp => cdp.DishProductId == dishProduct.Id && cdp.MealDishId == mealDishId)
                .SingleOrDefaultAsync();

            if(customizedDishProduct is null)
            {
                var newCustomizedDishProduct = new Database.Models.CustomizedDishProducts
                {
                    DishProductId = dishProduct.Id,
                    CustomizedPortionMultiplier = customizedMultiplier,
                    MealDishId = mealDishId
                };

                await _databaseContext.CustomizedDishProducts.AddAsync(newCustomizedDishProduct);
            }
            else
            {
                customizedDishProduct.CustomizedPortionMultiplier = customizedMultiplier;
            }

            await _databaseContext.SaveChangesAsync();

            return new DatabaseActionResult(true);
        }
    }
}