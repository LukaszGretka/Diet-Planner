using DietPlanner.Application.Interfaces.Repository;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;

namespace DietPlanner.Infrastructure.Repositories
{
    public class DishProductRepository : GenericRepository<DishProduct>, IDishProductRepository
    {
        public DishProductRepository(DietPlannerDbContext dbContext) : base(dbContext)
        {
        }

    }
}
