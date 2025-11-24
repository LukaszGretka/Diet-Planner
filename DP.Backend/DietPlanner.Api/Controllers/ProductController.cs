using DietPlanner.Api.DTO.Products;
using DietPlanner.Api.Services;
using DietPlanner.Domain.Entities;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            return await _productService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _productService.GetById(id);

            if (product is null)
            {
                return NotFound(new { Message = $"Product with id {id} no found" });
            }

            return product;
        }

        [HttpGet]
        public async Task<ActionResult<Product>> GetByName([FromQuery] string productName)
        {
            if (productName is null)
            {
                return BadRequest($"Missing parameter: '{nameof(productName)}'");
            }
            var product = await _productService.GetByName(productName);

            if (product is null)
            {
                return NotFound(new { Message = $"Product with name '{productName}' not found" });
            }

            return product;
        }

        [HttpPost]
        [ActionName(nameof(PostAsync))]
        public async Task<IActionResult> PostAsync([FromBody] Product product)
        {
            DatabaseActionResult<Product> result = await _productService.Create(product);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction(nameof(PostAsync), new { id = result.Obj.Id }, result.Obj);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            DatabaseActionResult<Product> result = await _productService.Update(id, product);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (!result.Success)
            {
                return NotFound(new { Message = "Product no found" });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            DatabaseActionResult<Product> result = await _productService.DeleteById(id);

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
}
