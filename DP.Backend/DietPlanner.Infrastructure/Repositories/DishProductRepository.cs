using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Infrastructure.Repositories;

public class DishProductRepository : GenericRepository<DishProduct>, IDishProductRepository
{
    public DishProductRepository(DietPlannerDbContext dbContext, 
        ILogger<DishProductRepository> logger) : base(dbContext, logger)
    {
    }

}
