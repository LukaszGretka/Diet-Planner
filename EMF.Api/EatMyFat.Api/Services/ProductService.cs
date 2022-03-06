using EatMyFat.Api.Models;
using System;
using System.Collections.Generic;

namespace EatMyFat.Api.Services
{
    public class ProductService : IProductService
    {
        public DatabaseActionResult<Product> Create(Product product)
        {
            throw new NotImplementedException();
        }

        public DatabaseActionResult<Product> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public DatabaseActionResult<Product> Update(int id, Product product)
        {
            throw new NotImplementedException();
        }
    }
}
