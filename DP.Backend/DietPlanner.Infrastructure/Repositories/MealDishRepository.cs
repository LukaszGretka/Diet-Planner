using DietPlanner.Application.Interfaces.Repository;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DietPlanner.Infrastructure.Repositories
{
    public class MealDishRepository : GenericRepository<MealDish>, IMealDishRepository
    {
        public MealDishRepository(DietPlannerDbContext dbContext) : base(dbContext) { }

        public async Task<List<MealDish>> GetAllAsync(int mealId, CancellationToken ct)
        {
            return await dbContext.MealDishes
                .Where(md => md.MealId == mealId)
                .Include(md => md.Dish)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
