using DietPlanner.Application.Interfaces.Common;
using DietPlanner.Domain.Entities;

namespace DietPlanner.Application.Interfaces.Repository
{
    public interface IMeasurementRepository : IGenericRepository<UserMeasurement>
    {
        Task<List<UserMeasurement>> GetAllByIdAsync(string userId, CancellationToken ct);

        Task<UserMeasurement?> GetByUserAndMeasurementIdAsync(string userId, int measurementId, CancellationToken ct);
    }
}
