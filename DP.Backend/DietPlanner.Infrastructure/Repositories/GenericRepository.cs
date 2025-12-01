using DietPlanner.Application.Interfaces.Common;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DietPlanner.Infrastructure.Repositories
{
    public class GenericRepository<T>(DietPlannerDbContext dbContext) : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DietPlannerDbContext dbContext = dbContext;
        private readonly DbSet<T> _dbSet = dbContext.Set<T>();

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct)
        {
            return await _dbSet.AsNoTracking().ToListAsync(ct);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, ct);
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

        public async Task AttachRangeAsync(IEnumerable<T> entity, CancellationToken ct)
        {
            _dbSet.AttachRange(entity);
            await dbContext.SaveChangesAsync(ct);
        }
    }
}