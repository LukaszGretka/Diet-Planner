using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities.Dishes;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DietPlanner.Infrastructure.Repositories
{
    public class DishRepository(DietPlannerDbContext dbContext) : GenericRepository<Dish>(dbContext), IDishRepository
    {
        public async Task<Dish?> GetByNameAsync(string name)
        {
            return await dbContext.Dishes.AsNoTracking().FirstOrDefaultAsync(dish => dish.Name.Equals(name));
        }
    }
}
