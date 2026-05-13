using DietPlanner.Application.Models.Products;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Models;

namespace DietPlanner.Application.Interfaces.Services;

public interface IProductService
{
    Task<List<ProductDTO>> GetAllAsync(CancellationToken ct);
    Task<Product?> GetByIdAsync(int id, CancellationToken ct);
    Task<Product?> GetByNameAsync(string name, CancellationToken ct);
    Task<DatabaseActionResult<Product>> CreateAsync(Product product, CancellationToken ct);
    Task<DatabaseActionResult<Product>> UpdateAsync(int id, Product product, CancellationToken ct);
    Task<DatabaseActionResult<Product>> DeleteByIdAsync(int id, CancellationToken ct);
}
