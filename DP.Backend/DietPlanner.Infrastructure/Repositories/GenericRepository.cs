using DietPlanner.Application.Interfaces.Repositories;
using DietPlanner.Domain.Entities;
using DietPlanner.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DietPlanner.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T: BaseEntity
    {
        private readonly DietPlannerDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DietPlannerDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            var result = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _context.Entry(entity).Entity;
        }

        public async Task AttachRangeAsync(IEnumerable<T> entity)
        {
            _dbSet.AttachRange(entity);
            await _context.SaveChangesAsync();
        }
    }
}
