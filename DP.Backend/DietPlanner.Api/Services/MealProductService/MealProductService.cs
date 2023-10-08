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

        public async Task<DatabaseActionResult> UpdateCustomizedPortionMultiplier(int dishId, int productId, decimal customizedMultiplier)
        {

            var dishProduct = await _databaseContext.DishProducts
                .Where(dp => dp.DishId == dishId && dp.ProductId == productId)
                .SingleAsync();

            dishProduct.CustomizedPortionMultiplier = customizedMultiplier;

            _databaseContext.DishProducts.Update(dishProduct);
            await _databaseContext.SaveChangesAsync();

            return new DatabaseActionResult(true);
        }
    }
}