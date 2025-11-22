using DietPlanner.Application.DTO.Products;
using DietPlanner.Domain.Entities.Products;

namespace DietPlanner.Api.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAll();

        Task<Product> GetById(int id);

        Task<Product> GetByName(string name);

        Task<DatabaseActionResult<Product>> Create(Product product);

        Task<DatabaseActionResult<Product>> Update(int id, Product product);

        Task<DatabaseActionResult<Product>> DeleteById(int id);
    }
}
