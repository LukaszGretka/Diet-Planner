using EatMyFat.Api.Models;
using System.Collections.Generic;

namespace EatMyFat.Api.Services
{
    interface IProductService
    {
        List<Product> GetAll();

        DatabaseActionResult<Product> Create(Product product);

        DatabaseActionResult<Product> Update(int id, Product product);

        DatabaseActionResult<Product> DeleteById(int id);
    }
}
