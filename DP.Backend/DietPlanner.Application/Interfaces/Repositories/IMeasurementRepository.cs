using DietPlanner.Domain.Repositories;
using DietPlanner.Domain.Entities;

namespace DietPlanner.Application.Interfaces.Repositories;

public interface IMeasurementRepository : IRepository<UserMeasurement>
{
    Task<List<UserMeasurement>> GetAllByIdAsync(string userId, CancellationToken ct);

    Task<UserMeasurement?> GetByUserAndMeasurementIdAsync(string userId, int measurementId, CancellationToken ct);
}
