using EatMyFat.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EatMyFat.Api.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAll();

        Task<Product> GetById(int id);

        Task<DatabaseActionResult<Product>> Create(Product product);

        Task<DatabaseActionResult<Product>> Update(int id, Product product);

        Task<DatabaseActionResult<Product>> DeleteById(int id);
    }
}
