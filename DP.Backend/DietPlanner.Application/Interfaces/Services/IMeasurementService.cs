using DietPlanner.Application.Models.UserMeasurement;

namespace DietPlanner.Application.Interfaces.Services;

public interface IMeasurementService
{
    Task<List<MeasurementDto>> GetAll(string userId, CancellationToken ct);

    Task<MeasurementDto?> GetById(int id, string userId, CancellationToken ct);

    Task<MeasurementDto?> Create(MeasurementDto measurement, string userId, CancellationToken ct);

    Task<MeasurementDto?> Update(int measurementId, MeasurementDto measurement, string userId, CancellationToken ct);

    Task<bool> DeleteById(int measurementId, string userId, CancellationToken ct);
}
