using DietPlanner.Application.Extensions;
using DietPlanner.Application.Interfaces;
using DietPlanner.Application.Interfaces.Repository;
using DietPlanner.Application.Models.UserMeasurement;
using DietPlanner.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Application.Services
{
    public class MeasurementService(ILogger<MeasurementService> logger, 
        IMeasurementRepository measurementRepository) : IMeasurementService
    {
        public async Task<List<MeasurementDto>> GetAll(string userId, CancellationToken ct)
        {
            List<UserMeasurement> userMeasurements = await measurementRepository.GetAllByIdAsync(userId, ct);
            var measurementsDto = new List<MeasurementDto>();

            return userMeasurements.Select(x => new MeasurementDto
            {
                Belly = x.Belly,
                BicepsLeft = x.BicepsLeft,
                BicepsRight = x.BicepsRight,
                CalfLeft = x.CalfLeft,
                CalfRight = x.CalfRight,
                Chest = x.Chest,
                Date = x.Date,
                ForearmLeft = x.ForearmLeft,
                ForearmRight = x.ForearmRight,
                ThighLeft = x.ThighLeft,
                ThighRight = x.ThighRight,
                Waist = x.Waist,
                Weight = x.Weight
            }).ToList();
        }

        public async Task<MeasurementDto?> GetById(int id, string userId, CancellationToken ct)
        {
            UserMeasurement? measurement = await measurementRepository
                .GetByUserAndMeasurementIdAsync(userId, id, ct);

            if(measurement is null)
            {
                logger.LogError("Measurement with id {MeasurementId} for user {UserId} not found", id, userId);
                return null;
            }

            return new MeasurementDto
            {
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

        public async Task<MeasurementDto?> Create(MeasurementDto measurement, string userId, CancellationToken ct)
        {
            UserMeasurement entity = new()
            {
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
                Weight = measurement.Weight,
                UserId = userId
            };

            UserMeasurement? createResult = await measurementRepository.CreateAsync(entity, ct);

            if (createResult is null)
            {
                logger.LogError("Failed to create measurement for user {UserId}", userId);
                return null;
            }

            return measurement;
        }

        public async Task<bool> DeleteById(int measurementId, string userId, CancellationToken ct)
        {
            UserMeasurement? measurement = await measurementRepository.GetByUserAndMeasurementIdAsync(userId, measurementId, ct);

            if (measurement is null)
            {
                logger.LogError("Measurement with id {MeasurementId} for user {UserId} not found", measurementId, userId);
                return false;
            }

            await measurementRepository.DeleteAsync(measurement, ct);

            return true;
        }

        public async Task<MeasurementDto?> Update(int measurementId, MeasurementDto measurement, string userId, CancellationToken ct)
        {
            UserMeasurement? currentMeasurement = await measurementRepository.GetByUserAndMeasurementIdAsync(userId, measurementId, ct);

            if (currentMeasurement is null)
            {
                logger.LogError("Measurement with id {MeasurementId} for user {UserId} not found", measurementId, userId);
                return null;
            }

            currentMeasurement.Weight = measurement.Weight;
            currentMeasurement.Chest = measurement.Chest;
            currentMeasurement.Belly = measurement.Belly;
            currentMeasurement.Waist = measurement.Waist;
            currentMeasurement.BicepsRight = measurement.BicepsRight;
            currentMeasurement.BicepsLeft = measurement.BicepsLeft;
            currentMeasurement.ForearmRight = measurement.ForearmRight;
            currentMeasurement.ForearmLeft = measurement.ForearmLeft;
            currentMeasurement.ThighRight = measurement.ThighRight;
            currentMeasurement.ThighLeft = measurement.ThighLeft;
            currentMeasurement.CalfRight = measurement.CalfRight;
            currentMeasurement.CalfLeft = measurement.CalfLeft;
            currentMeasurement.Date = System.DateTime.UtcNow.ToDatabaseDateFormat();

            await measurementRepository.UpdateAsync(currentMeasurement, ct);

            return measurement;
        }
    }
}
