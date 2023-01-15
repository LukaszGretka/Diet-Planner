using DietPlanner.Api.Database;
using DietPlanner.Api.Models;
using DietPlanner.Shared.Models;
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
        public async Task<List<UserMeasurement>> GetAll()
        {
            return await _databaseContext.UserMeasurements.AsNoTracking().ToListAsync();
        }

        public async Task<UserMeasurement> GetById(int id)
        {
            return await _databaseContext.UserMeasurements.FindAsync(id);
        }

        public async Task<DatabaseActionResult<UserMeasurement>> Create(UserMeasurement measurement)
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
                return new DatabaseActionResult<UserMeasurement>(false, exception: ex);
            }

            return new DatabaseActionResult<UserMeasurement>(true, obj: measurement);
        }

        public async Task<DatabaseActionResult<UserMeasurement>> DeleteById(int id)
        {
            UserMeasurement foundMeasurement = await _databaseContext.UserMeasurements.FindAsync(id);

            if (foundMeasurement is null)
            {
                return new DatabaseActionResult<UserMeasurement>(false, "Measurement no found");
            }

            try
            {
                _databaseContext.UserMeasurements.Remove(foundMeasurement);
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<UserMeasurement>(false, exception: ex);
            }

            return new DatabaseActionResult<UserMeasurement>(true);
        }

        public async Task<DatabaseActionResult<UserMeasurement>> Update(int id, UserMeasurement measurement)
        {
            UserMeasurement existingMeasurment = await _databaseContext.UserMeasurements.FindAsync(id);

            if (existingMeasurment is null)
            {
                return new DatabaseActionResult<UserMeasurement>(false, "Measurement no found");
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
                return new DatabaseActionResult<UserMeasurement>(false, exception: ex);
            }

            return new DatabaseActionResult<UserMeasurement>(true);
        }
    }
}
