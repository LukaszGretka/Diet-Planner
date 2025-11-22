using DietPlanner.Api.Services.Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DietPlanner.Application.Services
{
    public class RedisCacheService(IDistributedCache distributedCache, ILogger<RedisCacheService> logger) : IRedisCacheService
    {
        private const int defaultCacheTimeInMinutes = 5;
        private readonly IDistributedCache _distributedCache = distributedCache;
        private readonly ILogger<RedisCacheService> _logger = logger;

        public async Task<string> GetAsync(string key, CancellationToken ct)
        {
            try
            {
                return await _distributedCache.GetStringAsync(key, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting value from Redis cache for key: {key}");
                return string.Empty;
            }
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken ct, TimeSpan? expiry = null)
        {   
            try
            {
                await _distributedCache.SetStringAsync(key,
                    JsonSerializer.Serialize(value),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(defaultCacheTimeInMinutes)
                    }, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while setting value in Redis cache for key: {key}");
                return;
            }
        }

        public async Task RemoveAsync(string key, CancellationToken ct)
        {
            try
            {
                await _distributedCache.RemoveAsync(key, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while removing value from Redis cache for key: {key}");
                return;
            }
        }
    }
}
