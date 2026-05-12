using DietPlanner.Domain.Repositories;
using DietPlanner.Domain.Entities.Base;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace DietPlanner.Infrastructure.Repositories;

public class GenericRepository<T>(DietPlannerDbContext dbContext,
    ILogger<GenericRepository<T>> logger) : IRepository<T> where T : BaseEntity
{
    protected readonly DietPlannerDbContext dbContext = dbContext;
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task<IReadOnlyList<T>?> GetAllAsync(CancellationToken ct)
    {
        try
        {
            return await _dbSet.AsNoTracking().ToListAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all entities of type {EntityType}", typeof(T).Name);
            return null;
        }
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct)
    {
        try
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving entity of type {EntityType} with ID {EntityId}", typeof(T).Name, id);
            return null;
        }
    }

    public async Task<T?> CreateAsync(T entity, CancellationToken ct)
    {
        try
        {
            EntityEntry<T> result = await _dbSet.AddAsync(entity, ct);
            await dbContext.SaveChangesAsync(ct);
            return result.Entity;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating entity of type {EntityType}", typeof(T).Name);
            return null;
        }
    }

    public async Task<T?> UpdateAsync(T entity, CancellationToken ct)
    {
        try
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync(ct);

            return dbContext.Entry(entity).Entity;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating entity of type {EntityType} with ID {EntityId}", typeof(T).Name, entity.Id);
            return null;
        }
    }

    public async Task<bool> DeleteAsync(T entity, CancellationToken ct)
    {
        try
        {
            _dbSet.Remove(entity);
            await dbContext.SaveChangesAsync(ct);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting entity of type {EntityType} with ID {EntityId}", typeof(T).Name, entity.Id);
            return false;
        }
    }

    public async Task AttachRangeAsync(IEnumerable<T> entity, CancellationToken ct)
    {
        try
        {
            _dbSet.AttachRange(entity);
            await dbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error attaching range of entities of type {EntityType}", typeof(T).Name);
        }
    }
}