using EatMyFat.Api.Models;
using EatMyFat.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EatMyFat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productService.GetAll();
        }

        [HttpGet]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product =  await _productService.GetById(id);

            if (product is null)
            {
                return NotFound(new { Message = $"Product with id {id} no found" });
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
    }
}
