using DietPlanner.Application.DTO;
using DietPlanner.Application.DTO.Dishes;
using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Infrastructure.Database;

namespace DietPlanner.Infrastructure.Repositories
{
    public class DashboardRepository(DietPlannerDbContext dbContext) : IDashboardRepository
    {
        public async Task<List<DatedDishProductsDto>> GetDatedDishProducts(string userId, DateTime dataTimeNow)
        {
            return dbContext.Meals
                .Join(dbContext.MealDishes, m => m.Id, md => md.MealId, (m, md) => new { m.Date, m.UserId, md.DishId, mealDishId = md.Id })
                    .Where(m => m.UserId == userId && (m.Date >= dataTimeNow.AddDays(-7) && m.Date <= dataTimeNow))
                    .GroupBy(m => new { m.Date })
                    .Select(gd => new DatedDishProductsDto
                    {
                        Date = gd.Key.Date,
                        DishProducts = gd.Join(dbContext.DishProducts, x => x.DishId, dp => dp.DishId, (x, dp) => new { x.DishId, x.mealDishId, dp })
                        .Join(dbContext.Products, x => x.dp.ProductId, p => p.Id, (x, p) => new DishProductsDTO
                        {
                            Product = p,
                            PortionMultiplier = x.dp.PortionMultiplier,
                            CustomizedPortionMultiplier = dbContext.CustomizedMealDishes
                                .Where(e => e.MealDishId == x.mealDishId && e.DishProductId == x.dp.Id)
                                .SingleOrDefault().CustomizedPortionMultiplier
                        })
                    }).ToList();
        }
    }
}
