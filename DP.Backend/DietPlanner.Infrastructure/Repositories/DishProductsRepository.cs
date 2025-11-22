
using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities.Dishes;
using DietPlanner.Infrastructure.Database;

namespace DietPlanner.Infrastructure.Repositories
{
    public class DishProductsRepository(DietPlannerDbContext dbContext) : GenericRepository<DishProducts>(dbContext), 
        IDishProductRepository
    {
    }
}
