using DietPlanner.Api.Database;
using DietPlanner.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly ILogger<MeasurementService> _logger;
        private readonly DatabaseContext _databaseContext;

        public MeasurementService(ILogger<MeasurementService> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        public async Task<DatabaseActionResult<Measurement>> Create(Measurement measurement)
        {
            measurement.Date = System.DateTime.Now.ToString();

            try
            {
                await _databaseContext.AddAsync(measurement);
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Measurement>(false, exception: ex);
            }

            return new DatabaseActionResult<Measurement>(true, obj: measurement);
        }

        public async Task<DatabaseActionResult<Measurement>> DeleteById(int id)
        {
            Measurement foundMeasurement = await _databaseContext.Measurements.FindAsync(id);

            if (foundMeasurement is null)
            {
                return new DatabaseActionResult<Measurement>(false, "Measurement no found");
            }

            try
            {
                _databaseContext.Measurements.Remove(foundMeasurement);
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Measurement>(false, exception: ex);
            }

            return new DatabaseActionResult<Measurement>(true);
        }

        public async Task<List<Measurement>> GetAll()
        {
            return await _databaseContext.Measurements.AsNoTracking().ToListAsync();
        }

        public async Task<DatabaseActionResult<Measurement>> Update(int id, Measurement measurement)
        {
            Measurement existingMeasurment = await _databaseContext.Measurements.FindAsync(id);

            if (existingMeasurment is null)
            {
                return new DatabaseActionResult<Measurement>(false, "Measurement no found");
            }

            existingMeasurment.Weight = measurement.Weight;
            existingMeasurment.Chest = measurement.Chest;
            existingMeasurment.Belly = measurement.Belly;
            existingMeasurment.Waist = measurement.Waist;
            existingMeasurment.BicepsRight = measurement.BicepsRight;
            existingMeasurment.BicepsLeft = measurement.BicepsLeft;
            existingMeasurment.ForearmRight = measurement.ForearmRight;
            existingMeasurment.ForearmLeft = measurement.ForearmLeft;
            existingMeasurment.ThighRight = measurement.ThighRight;
            existingMeasurment.ThighLeft = measurement.ThighLeft;
            existingMeasurment.CalfRight = measurement.CalfRight;
            existingMeasurment.CalfLeft = measurement.CalfLeft;

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<Measurement>(false, exception: ex);
            }

            return new DatabaseActionResult<Measurement>(true);
        }
    }
}
