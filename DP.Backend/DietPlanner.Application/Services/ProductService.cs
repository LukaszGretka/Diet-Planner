using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Application.Interfaces.Services;
using DietPlanner.Application.Models.Products;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Application.Services;

public class ProductService(
    IProductRepository productRepository,
    IDishProductRepository dishProductRepository,
    ILogger<ProductService> logger) : IProductService
{
    public async Task<List<ProductDTO>> GetAllAsync(CancellationToken ct)
    {
        var products = await productRepository.GetAllAsync(ct);

        if (products is null)
        {
            return new List<ProductDTO>();
        }

        return products
            .Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BarCode = p.BarCode,
                ImagePath = p.ImagePath,
                Calories = (float)p.Calories.GetValueOrDefault(),
                Carbohydrates = (float)p.Carbohydrates.GetValueOrDefault(),
                Proteins = (float)p.Proteins.GetValueOrDefault(),
                Fats = (float)p.Fats.GetValueOrDefault()
            }).ToList();
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await productRepository.GetByIdAsync(id, ct);
    }

    public async Task<Product?> GetByNameAsync(string name, CancellationToken ct)
    {
        return await productRepository.GetByNameAsync(name, ct);
    }

    public async Task<DatabaseActionResult<Product>> CreateAsync(Product product, CancellationToken ct)
    {
        var createdProduct = await productRepository.CreateAsync(product, ct);

        if (createdProduct is null)
        {
            return new DatabaseActionResult<Product>(false, "Failed to create product");
        }

        return new DatabaseActionResult<Product>(true, obj: createdProduct);
    }

    public async Task<DatabaseActionResult<Product>> UpdateAsync(int id, Product product, CancellationToken ct)
    {
        var existingProduct = await productRepository.GetByIdAsync(id, ct);
        if (existingProduct is null)
        {
            return new DatabaseActionResult<Product>(false, "Product not found");
        }

        existingProduct.Name = string.IsNullOrWhiteSpace(product.Name) ? existingProduct.Name : product.Name;
        existingProduct.Description = product.Description;
        existingProduct.BarCode = product.BarCode ?? existingProduct.BarCode;
        existingProduct.ImagePath = string.IsNullOrWhiteSpace(product.ImagePath) ? existingProduct.ImagePath : product.ImagePath;
        existingProduct.Calories = product.Calories ?? existingProduct.Calories;
        existingProduct.Carbohydrates = product.Carbohydrates ?? existingProduct.Carbohydrates;
        existingProduct.Fats = product.Fats ?? existingProduct.Fats;
        existingProduct.Proteins = product.Proteins ?? existingProduct.Proteins;

        var updatedProduct = await productRepository.UpdateAsync(existingProduct, ct);
        if (updatedProduct is null)
        {
            return new DatabaseActionResult<Product>(false, "Failed to update product");
        }
        return new DatabaseActionResult<Product>(true, obj: updatedProduct);
    }

    public async Task<DatabaseActionResult<Product>> DeleteByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            var foundProduct = await productRepository.GetByIdAsync(id, ct);
            if (foundProduct is null)
            {
                return new DatabaseActionResult<Product>(false, "Product not found");
            }

            var isProductAssigned = await dishProductRepository.IsProductAssignedToDishAsync(id, ct);
            if (isProductAssigned)
            {
                return new DatabaseActionResult<Product>(false,
                    $"Product '{foundProduct.Name}' can't be deleted because it's used in one of the dishes.");
            }

            var deleted = await productRepository.DeleteAsync(foundProduct, ct);
            if (!deleted)
            {
                return new DatabaseActionResult<Product>(false, "Failed to delete product");
            }
            return new DatabaseActionResult<Product>(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting product with id {ProductId}", id);
            return new DatabaseActionResult<Product>(false, exception: ex);
        }
    }
}
