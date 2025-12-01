using DietPlanner.Application.Interfaces.Repository;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DietPlanner.Infrastructure.Repositories
{
    public class CustomizedMealProductRepository : GenericRepository<CustomizedMealProducts>, ICustomizedMealProductRepository
    {
        public CustomizedMealProductRepository(DietPlannerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<decimal> GetPortionMultiplierAsync(int mealProductId, CancellationToken ct)
        {
            return dbContext.CustomizedMealProducts.Where(cmp => cmp.MealProductId == mealProductId)
            .Select(cmp => cmp.CustomizedPortionMultiplier)
            .FirstOrDefaultAsync(ct);
        }
    }
}
