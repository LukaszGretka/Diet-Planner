using DietPlanner.Api.Models;
using DietPlanner.Api.Services;
using DietPlanner.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    public class MeasurementController : Controller
    {
        private readonly IMeasurementService _measurementService;

        public MeasurementController(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
        }

        [HttpGet]
        public async Task<IEnumerable<Measurement>> GetAllAsync()
        {
            return await _measurementService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Measurement>> GetById(int id)
        {
            Measurement measurement = await _measurementService.GetById(id);

            if (measurement is null)
            {
                return NotFound(new { Message = $"Measurement with id {id} no found" });
            }

            return measurement;
        }

        [HttpPost]
        [ActionName(nameof(PostAsync))]
        public async Task<IActionResult> PostAsync([FromBody] Measurement measurement)
        {
            if (measurement == null)
            {
                return BadRequest();
            }

            DatabaseActionResult<Measurement> result = await _measurementService.Create(measurement);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction(nameof(PostAsync), new { id = result.Obj.Id }, result.Obj);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Measurement measurement)
        {
            if (id != measurement.Id)
            {
                return BadRequest();
            }

            DatabaseActionResult<Measurement> result = await _measurementService.Update(id, measurement);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (!result.Success)
            {
                return NotFound(new { Message = "Measurement no found" });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            DatabaseActionResult<Measurement> result = await _measurementService.DeleteById(id);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (!result.Success)
            {
                return NotFound(new { Message = "Measurement no found" });
            }

            return NoContent();
        }
    }
}
