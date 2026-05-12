namespace DietPlanner.Application.Interfaces.Services;

public interface IRedisCacheService
{
    public Task<string?> GetAsync(string key, CancellationToken ct);

    public Task SetAsync<T>(string key, T value, CancellationToken ct, TimeSpan? expiry = null);

    public Task RemoveAsync(string key, CancellationToken ct);
}
