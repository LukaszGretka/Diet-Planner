using DietPlanner.Api.Database;
using DietPlanner.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly DatabaseContext _databaseContext;

        public ProductService(ILogger<ProductService> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<DatabaseActionResult<Product>> Create(Product product)
        {
            try
            {
                await _databaseContext.AddAsync(product);
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Product>(false, exception: ex);
            }

            return new DatabaseActionResult<Product>(true, obj: product);
        }

        public async Task<DatabaseActionResult<Product>> DeleteById(int id)
        {
            Product foundProduct = await _databaseContext.Products.FindAsync(id);

            if (foundProduct is null)
            {
                return new DatabaseActionResult<Product>(false, "Product no found");
            }

            try
            {
                _databaseContext.Products.Remove(foundProduct);
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Product>(false, exception: ex);
            }

            return new DatabaseActionResult<Product>(true);
        }

        public async Task<List<Product>> GetAll()
        {
            return await _databaseContext.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _databaseContext.Products.FindAsync(id);
        }

        public async Task<DatabaseActionResult<Product>> Update(int id, Product product)
        {
            Product existingProduct = await _databaseContext.Products.FindAsync(product.Id);

            if (existingProduct is null)
            {
                return new DatabaseActionResult<Product>(false, "Product no found");
            }

            existingProduct.Name = string.IsNullOrWhiteSpace(product.Name) ? existingProduct.Name : product.Name;
            existingProduct.Description = string.IsNullOrWhiteSpace(product.Description) ? existingProduct.Description : product.Description;
            existingProduct.BarCode = product.BarCode ?? existingProduct.BarCode;
            existingProduct.ImagePath = string.IsNullOrWhiteSpace(product.ImagePath) ? existingProduct.ImagePath : product.ImagePath;
            existingProduct.Calories = product.Calories ?? existingProduct.Calories;
            existingProduct.Carbohydrates = product.Carbohydrates ?? product.Carbohydrates;
            existingProduct.Fats = product.Fats ?? product.Fats;
            existingProduct.Proteins = product.Proteins ?? product.Proteins;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Product>(false, exception: ex);
            }

            return new DatabaseActionResult<Product>(true);
        }
    }
}
