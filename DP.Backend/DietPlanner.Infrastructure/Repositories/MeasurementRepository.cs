using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Infrastructure.Repositories;

public class MeasurementRepository(DietPlannerDbContext dbContext, ILogger<MeasurementRepository> logger) : GenericRepository<UserMeasurement>(dbContext, logger), IMeasurementRepository
{
    public async Task<List<UserMeasurement>> GetAllByIdAsync(string userId, CancellationToken ct)
    {
        try
        {
            return await dbContext.Measurements
                .Where(m => m.UserId.Equals(userId))
                .AsNoTracking()
                .ToListAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving measurements for user {UserId}", userId);
            return [];
        }
    }

    public Task<UserMeasurement?> GetByUserAndMeasurementIdAsync(string userId, int measurementId, CancellationToken ct)
    {
        return dbContext.Measurements
            .SingleOrDefaultAsync(measurement =>
            measurement.UserId == userId && measurement.Id == measurementId, ct);
    }
}
