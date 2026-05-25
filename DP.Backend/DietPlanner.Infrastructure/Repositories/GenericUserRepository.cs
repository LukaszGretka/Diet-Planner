using DietPlanner.Domain.Repositories;
using DietPlanner.Domain.Entities.Base;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DietPlanner.Infrastructure.Repositories;

public class GenericUserRepository<T>(DietPlannerDbContext dbContext) : IUserRepository<T> where T : BaseUserEntity
{
    protected readonly DietPlannerDbContext dbContext = dbContext;
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().ToListAsync(ct);
    }

    public async Task<T?> GetByIdAsync(string userId, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(t => t.UserId == userId, ct);
    }

    public async Task<T> CreateAsync(T entity, CancellationToken ct)
    {
        var result = await _dbSet.AddAsync(entity, ct);
        await dbContext.SaveChangesAsync(ct);

        return result.Entity;
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken ct)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(ct);

        return dbContext.Entry(entity).Entity;
    }

    public async Task DeleteAsync(T entity, CancellationToken ct)
    {
        _dbSet.Remove(entity);
        await dbContext.SaveChangesAsync(ct);
    }
}
