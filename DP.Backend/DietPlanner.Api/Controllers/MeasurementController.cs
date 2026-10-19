using DietPlanner.Api.Extensions;
using DietPlanner.Application.Interfaces.Services;
using DietPlanner.Application.Models.UserMeasurement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace DietPlanner.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MeasurementController(IMeasurementService measurementService) : Controller
{
    [HttpGet]
    public async Task<List<MeasurementDto>> GetAllAsync(CancellationToken ct)
    {
        string userId = HttpContext.GetUserId();

        return await measurementService.GetAll(userId, ct);
    }

    [HttpGet("{measurementId}")]
    public async Task<ActionResult<MeasurementDto>> GetById(int measurementId, CancellationToken ct)
    {
        string userId = HttpContext.GetUserId();

        MeasurementDto measurement = await measurementService.GetById(measurementId, userId, ct);

        if (measurement is null)
        {
            return NotFound(new { Message = $"Measurement with id '{measurementId}' no found" });
        }

        return measurement;
    }

    [HttpPost]
    [ActionName(nameof(AddMeasurement))]
    public async Task<IActionResult> AddMeasurement([FromBody][Required] MeasurementDto measurement, CancellationToken ct)
    {
        string userId = HttpContext.GetUserId();

        MeasurementDto result = await measurementService.Create(measurement, userId, ct);

        if (result is null)
        {
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }

        return CreatedAtAction(nameof(AddMeasurement), result);
    }

    [HttpPut("{measurementId}")]
    public async Task<IActionResult> UpdateMeasurement(int measurementId, [FromBody][Required] MeasurementDto measurement, CancellationToken ct)
    {
        string userId = HttpContext.GetUserId();

        MeasurementDto result = await measurementService.Update(measurementId, measurement, userId, ct);

        if (result is null)
        {
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        string userId = HttpContext.GetUserId();

        bool success = await measurementService.DeleteById(id, userId, ct);

        if (!success)
        {
            return BadRequest(new { Message = "Unable to delete measurement" });
        }

        return NoContent();
    }
}
