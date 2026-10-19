using DietPlanner.Application.Models.Products;
using DietPlanner.Application.Interfaces.Services;
using DietPlanner.Domain.Entities;
using DietPlanner.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController(IProductService productService) : ControllerBase
{

    [HttpGet]
    [Route("all")]
    public async Task<IEnumerable<ProductDTO>> GetAllAsync(CancellationToken ct)
    {
        return await productService.GetAllAsync(ct);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id, CancellationToken ct)
    {
        Product product = await productService.GetByIdAsync(id, ct);

        if (product is null)
        {
            return NotFound(new { Message = $"Product with id {id} not found" });
        }

        return product;
    }

    [HttpGet]
    public async Task<ActionResult<Product>> GetByName([FromQuery] string productName, CancellationToken ct)
    {
        if (productName is null)
        {
            return BadRequest($"Missing parameter: '{nameof(productName)}'");
        }

        Product product = await productService.GetByNameAsync(productName, ct);

        if (product is null)
        {
            return NotFound(new { Message = $"Product with name '{productName}' not found" });
        }

        return product;
    }

    [HttpPost]
    [ActionName(nameof(PostAsync))]
    public async Task<IActionResult> PostAsync([FromBody] Product product, CancellationToken ct)
    {
        DatabaseActionResult<Product> result = await productService.CreateAsync(product, ct);

        if (result.Exception != null)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(PostAsync), new { id = result.Obj.Id }, result.Obj);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Product product, CancellationToken ct)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        DatabaseActionResult<Product> result = await productService.UpdateAsync(id, product, ct);

        if (result.Exception != null)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        if (!result.Success)
        {
            return NotFound(new { result.Message });
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        DatabaseActionResult<Product> result = await productService.DeleteByIdAsync(id, ct);

        if (result?.Exception != null)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }

        return NoContent();
    }
}
