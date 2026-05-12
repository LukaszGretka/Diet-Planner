using DietPlanner.Domain.Entities.Base;

namespace DietPlanner.Domain.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>?> GetAllAsync(CancellationToken ct);

        Task<T?> GetByIdAsync(int id, CancellationToken ct);

        Task<T?> CreateAsync(T entity, CancellationToken ct);

        Task<T?> UpdateAsync(T entity, CancellationToken ct);

        Task<bool> DeleteAsync(T entity, CancellationToken ct);

        Task AttachRangeAsync(IEnumerable<T> entity, CancellationToken ct);
    }
}
