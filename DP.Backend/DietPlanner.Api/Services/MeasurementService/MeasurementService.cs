using DietPlanner.Api.Database;
using DietPlanner.Api.Database.Models;
using DietPlanner.Api.Extensions;
using DietPlanner.Api.Models.BodyProfile.DTO;
using DietPlanner.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietPlanner.Api.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly ILogger<MeasurementService> _logger;
        private readonly DietPlannerDbContext _databaseContext;

        public MeasurementService(ILogger<MeasurementService> logger, DietPlannerDbContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }
        public async Task<List<MeasurementDto>> GetAll(string userId)
        {
            var measurements = await _databaseContext
                .Measurements
                .AsNoTracking()
                .Where(m => m.UserId.Equals(userId))
                .ToListAsync();

            var measurementsDto = new List<MeasurementDto>();

            measurements.ForEach(m =>
            {
                measurementsDto.Add(new MeasurementDto
                {
                    Id = m.Id,
                    Belly = m.Belly,
                    BicepsLeft = m.BicepsLeft,
                    BicepsRight = m.BicepsRight,
                    CalfLeft = m.CalfLeft,
                    CalfRight = m.CalfRight,
                    Chest = m.Chest,
                    Date = m.Date,
                    ForearmLeft = m.ForearmLeft,
                    ForearmRight = m.ForearmRight,
                    ThighLeft = m.ThighLeft,
                    ThighRight = m.ThighRight,
                    Waist = m.Waist,
                    Weight = m.Weight
                });
            });

            return measurementsDto;
        }

        public async Task<MeasurementDto> GetById(int id, string userId)
        {
            var measurement = await _databaseContext
                .Measurements
                .SingleAsync(measurement => measurement.UserId.Equals(userId) && measurement.Id == id);

            return new MeasurementDto
            {
                Id = measurement.Id,
                Belly = measurement.Belly,
                BicepsLeft = measurement.BicepsLeft,
                BicepsRight = measurement.BicepsRight,
                CalfLeft = measurement.CalfLeft,
                CalfRight = measurement.CalfRight,
                Chest = measurement.Chest,
                Date = measurement.Date,
                ForearmLeft = measurement.ForearmLeft,
                ForearmRight = measurement.ForearmRight,
                ThighLeft = measurement.ThighLeft,
                ThighRight = measurement.ThighRight,
                Waist = measurement.Waist,
                Weight = measurement.Weight
            };
        }

        public async Task<DatabaseActionResult<MeasurementDto>> Create(MeasurementDto measurement, string userId)
        {
            try
            {
                await _databaseContext.AddAsync(new Measurement
                {
                    UserId = userId,
                    Belly = measurement.Belly,
                    BicepsLeft = measurement.BicepsLeft,
                    BicepsRight = measurement.BicepsRight,
                    CalfLeft = measurement.CalfLeft,
                    CalfRight = measurement.CalfRight,
                    Chest = measurement.Chest,
                    Date = System.DateTime.UtcNow.ToDatabaseDateFormat(),
                    ForearmLeft = measurement.ForearmLeft,
                    ForearmRight = measurement.ForearmRight,
                    ThighLeft = measurement.ThighLeft,
                    ThighRight = measurement.ThighRight,
                    Waist = measurement.Waist,
                    Weight = measurement.Weight
                });
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<MeasurementDto>(false, exception: ex);
            }

            return new DatabaseActionResult<MeasurementDto>(true, obj: measurement);
        }

        public async Task<DatabaseActionResult<MeasurementDto>> DeleteById(int measurementId, string userId)
        {
            Measurement foundMeasurement = await _databaseContext.Measurements
                .SingleAsync(measurement => measurement.UserId.Equals(userId) && measurement.Id == measurementId);

            if (foundMeasurement is null)
            {
                return new DatabaseActionResult<MeasurementDto>(false, "Measurement no found");
            }

            try
            {
                _databaseContext.Measurements.Remove(foundMeasurement);
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<MeasurementDto>(false, exception: ex);
            }

            return new DatabaseActionResult<MeasurementDto>(true);
        }

        public async Task<DatabaseActionResult<MeasurementDto>> Update(int measurementId, MeasurementDto measurement, string userId)
        {
            Measurement existingMeasurment = await _databaseContext.Measurements
                .SingleAsync(measurement => measurement.UserId.Equals(userId) && measurement.Id == measurementId);

            if (existingMeasurment is null)
            {
                return new DatabaseActionResult<MeasurementDto>(false, "Measurement no found");
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
            existingMeasurment.Date = System.DateTime.UtcNow.ToDatabaseDateFormat();

            try
            {
                await _databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex.Message);
                return new DatabaseActionResult<MeasurementDto>(false, exception: ex);
            }

            return new DatabaseActionResult<MeasurementDto>(true);
        }
    }
}
