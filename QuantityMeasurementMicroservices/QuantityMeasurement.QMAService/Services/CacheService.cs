using Microsoft.Extensions.Caching.Distributed;
using QuantityMeasurement.QMAService.Interfaces;

namespace QuantityMeasurement.QMAService.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheService> _logger;

        public CacheService(
            IDistributedCache cache,
            ILogger<CacheService> logger)
        {
            _cache  = cache;
            _logger = logger;
        }

        public async Task<string?> GetAsync(string key)
        {
            var value = await _cache.GetStringAsync(key);
            if (value != null)
                _logger.LogInformation(
                    "[Cache] HIT — key: {Key}", key);
            else
                _logger.LogInformation(
                    "[Cache] MISS — key: {Key}", key);
            return value;
        }

        public async Task SetAsync(
            string key, string value,
            TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    expiry ?? TimeSpan.FromMinutes(10)
            };
            await _cache.SetStringAsync(key, value, options);
            _logger.LogInformation(
                "[Cache] SET — key: {Key}", key);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
            _logger.LogInformation(
                "[Cache] REMOVE — key: {Key}", key);
        }
    }
}