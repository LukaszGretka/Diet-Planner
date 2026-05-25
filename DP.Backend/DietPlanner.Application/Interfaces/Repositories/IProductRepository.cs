using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Repositories;

namespace DietPlanner.Application.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByNameAsync(string name, CancellationToken ct);
}
