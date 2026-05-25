using DietPlanner.Domain.Entities.Base;

namespace DietPlanner.Domain.Repositories;

public interface IUserRepository<T> where T : BaseUserEntity
{
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct);

    Task<T?> GetByIdAsync(string userId, CancellationToken ct);

    Task<T> CreateAsync(T entity, CancellationToken ct);

    Task<T> UpdateAsync(T entity, CancellationToken ct);

    Task DeleteAsync(T entity, CancellationToken ct);
}
