using EatMyFat.Api.Models;
using EatMyFat.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EatMyFat.Api.Controllers
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
    }
}
