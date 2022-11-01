﻿using DietPlanner.Api.Models;
using DietPlanner.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services
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
