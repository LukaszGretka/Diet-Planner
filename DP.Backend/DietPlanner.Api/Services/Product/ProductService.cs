using DietPlanner.Api.Database;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.Models.MealsCalendar.DbModel;
using DietPlanner.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services
{
    public class ProductService: IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly DietPlannerDbContext _databaseContext;

        public ProductService(ILogger<ProductService> logger, DietPlannerDbContext databaseContext)
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
                var isProductAssignedToDish = _databaseContext.DishProducts
                    .Any(dishProduct => dishProduct.Id == id);

                if (isProductAssignedToDish)
                {
                    return new DatabaseActionResult<Product>
                        (false, $"Product '{foundProduct.Name}' can't be deleted because it's used in one of the dishes.");
                }

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

        public async Task<Product> GetByName(string name)
        {
            return await _databaseContext.Products.Where(product => product.Name.Equals(name)).FirstOrDefaultAsync();
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
