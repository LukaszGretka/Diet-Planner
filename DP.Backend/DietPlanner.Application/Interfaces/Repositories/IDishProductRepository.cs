using DietPlanner.Domain.Repositories;
using DietPlanner.Domain.Entities;

namespace DietPlanner.Application.Interfaces.Repositories;

public interface IDishProductRepository : IRepository<DishProduct>
{
    Task<bool> IsProductAssignedToDishAsync(int productId, CancellationToken ct);
}
