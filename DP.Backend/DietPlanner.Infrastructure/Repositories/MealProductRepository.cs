using DietPlanner.Application.Interfaces.Repository;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DietPlanner.Infrastructure.Repositories
{
    public class MealProductRepository : GenericRepository<MealProduct>, IMealProductRepository
    {
        public MealProductRepository(DietPlannerDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<List<MealProduct>> GetMealProducts(int mealId, CancellationToken ct)
        {
            return await dbContext.MealProducts.Where(mp => mp.MealId == mealId).ToListAsync(ct);
        }
    }
}
