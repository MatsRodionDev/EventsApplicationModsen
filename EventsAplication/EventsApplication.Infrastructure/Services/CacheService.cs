using EventsApplication.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace EventsApplication.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var jsonStringValue = await _distributedCache.GetStringAsync(key, cancellationToken);

            if (jsonStringValue is null)
            {
                return default;
            }

            var model = JsonSerializer.Deserialize<T>(jsonStringValue);

            return model;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null,  CancellationToken cancellationToken = default)
        {
            var jsonStringValue = JsonSerializer.Serialize(value);

            await _distributedCache.SetStringAsync(key, jsonStringValue, 
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expirationTime is null 
                    ? TimeSpan.FromDays(30) 
                    : expirationTime
                }, cancellationToken) ;
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            return _distributedCache.RemoveAsync(key, cancellationToken);
        }
    }
}

