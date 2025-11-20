using DietPlanner.Api.Extensions;
using DietPlanner.Api.Models.BodyProfile.DTO;
using DietPlanner.Api.Services;
using DietPlanner.Domain.Entities;
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
    public class MeasurementController : Controller
    {
        private readonly IMeasurementService _measurementService;

        public MeasurementController(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
        }

        [HttpGet]
        public async Task<IEnumerable<MeasurementDto>> GetAllAsync()
        {
            string userId = HttpContext.GetUserId();

            return await _measurementService.GetAll(userId);
        }

        [HttpGet("{measurementId}")]
        public async Task<ActionResult<MeasurementDto>> GetById(int measurementId)
        {
            string userId = HttpContext.GetUserId();

            MeasurementDto measurement = await _measurementService.GetById(measurementId, userId);

            if (measurement is null)
            {
                return NotFound(new { Message = $"Measurement with id '{measurementId}' no found" });
            }

            return measurement;
        }

        [HttpPost]
        [ActionName(nameof(AddMeasurement))]
        public async Task<IActionResult> AddMeasurement([FromBody] MeasurementDto measurement)
        {
            string userId = HttpContext.GetUserId();

            if (measurement is null || userId is null)
            {
                return BadRequest();
            }

            DatabaseActionResult<MeasurementDto> result = await _measurementService.Create(measurement, userId);

            if (result.Exception != null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction(nameof(AddMeasurement), new { id = result.Obj.Id }, result.Obj);
        }

        [HttpPut("{measurementId}")]
        public async Task<IActionResult> UpdateMeasurement(int measurementId, [FromBody] MeasurementDto measurement)
        {
            string userId = HttpContext.GetUserId();

            DatabaseActionResult<MeasurementDto> result = await _measurementService.Update(measurementId, measurement, userId);

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
            string userId = HttpContext.GetUserId();

            DatabaseActionResult<MeasurementDto> result = await _measurementService.DeleteById(id, userId);

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
