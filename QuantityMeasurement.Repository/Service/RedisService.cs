using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace QuantityMeasurement.Repository.Service
{
    /// <summary>
    /// Redis cache service for storing and retrieving data.
    /// Used to cache measurement results for fast retrieval.
    /// UC18
    /// </summary>
    public class RedisService
    {
        private readonly IDatabase _db;
        private readonly ILogger<RedisService> _logger;
        private readonly TimeSpan _defaultExpiry = TimeSpan.FromMinutes(30);

        public RedisService(
            IConfiguration config,
            ILogger<RedisService> logger)
        {
            _logger = logger;

            string connStr = config.GetConnectionString("Redis")
                ?? "localhost:6379";

            var connection = ConnectionMultiplexer.Connect(connStr);
            _db = connection.GetDatabase();

            _logger.LogInformation(
                "[RedisService] Connected to Redis at {Conn}", connStr);
        }

        // ── Set ───────────────────────────────────────────────

        /// <summary>
        /// Stores any object as JSON in Redis with optional expiry.
        /// UC18
        /// </summary>
        public async Task SetAsync<T>(
            string key, T value, TimeSpan? expiry = null)
        {
            string json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(
                key, json, expiry ?? _defaultExpiry);

            _logger.LogInformation(
                "[RedisService] SET key={Key}", key);
        }

        // ── Get ───────────────────────────────────────────────

        /// <summary>
        /// Retrieves and deserializes an object from Redis.
        /// Returns null if key does not exist.
        /// UC18
        /// </summary>
        public async Task<T?> GetAsync<T>(string key)
        {
            RedisValue value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                _logger.LogInformation(
                    "[RedisService] MISS key={Key}", key);
                return default;
            }

            _logger.LogInformation(
                "[RedisService] HIT key={Key}", key);

            return JsonSerializer.Deserialize<T>(value.ToString()!);
        }

        // ── Delete ────────────────────────────────────────────

        /// <summary>
        /// Removes a key from Redis.
        /// UC18
        /// </summary>
        public async Task DeleteAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
            _logger.LogInformation(
                "[RedisService] DEL key={Key}", key);
        }

        // ── Exists ────────────────────────────────────────────

        /// <summary>
        /// Checks if a key exists in Redis.
        /// UC18
        /// </summary>
        public async Task<bool> ExistsAsync(string key)
            => await _db.KeyExistsAsync(key);
    }
}