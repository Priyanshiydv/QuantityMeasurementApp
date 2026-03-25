using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using QuantityMeasurement.Models.Entities;
using QuantityMeasurement.Repository.Context;
using QuantityMeasurement.Repository.Interfaces;
using System.Text.Json;

namespace QuantityMeasurement.Repository.Service
{
    /// <summary>
    /// EF Core repository with Redis caching layer.
    /// WRITE → Save to SQL Server → Invalidate Redis cache
    /// READ  → Check Redis first (5-min TTL)
    ///         Cache HIT  → return from Redis
    ///         Cache MISS → query SQL Server → save to Redis
    /// UC17: EF Core, UC18: Redis caching added
    /// </summary>
    public class EFQuantityMeasurementRepository
        : IQuantityMeasurementRepository
    {
        private readonly QuantityMeasurementDbContext _context;
        private readonly IDistributedCache _cache;
        private readonly ILogger<EFQuantityMeasurementRepository> _logger;

        // Redis key prefix and TTL — same as friend's project
        private const string KeyAll = "qm:all";
        private static readonly TimeSpan CacheTtl =
            TimeSpan.FromMinutes(5);

        public EFQuantityMeasurementRepository(
            QuantityMeasurementDbContext context,
            IDistributedCache cache,
            ILogger<EFQuantityMeasurementRepository> logger)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _cache  = cache;
            _logger = logger;

            _logger.LogInformation("[EFRepository] Initialized ✓");
        }

        // ── Save → SQL Server + Invalidate Redis ──────────────

        /// <summary>
        /// Saves entity to SQL Server.
        /// Invalidates Redis cache so next read is fresh.
        /// UC17, UC18
        /// </summary>
        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.QuantityMeasurements.Add(entity);
            _context.SaveChanges();

            _logger.LogInformation(
                "[EFRepository] Saved to SQL Server: {Id}",
                entity.Id);

            // Invalidate Redis cache after every save
            InvalidateCache(entity.OperationType,
                            entity.MeasurementType);
        }

        // ── GetAllMeasurements → Redis first, SQL fallback ────

        /// <summary>
        /// Returns all measurements.
        /// Checks Redis cache first (5-min TTL).
        /// Falls back to SQL Server on cache miss.
        /// UC17, UC18
        /// </summary>
        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            // Try Redis first
            var cached = TryGetFromCache
                <List<QuantityMeasurementEntity>>(KeyAll);

            if (cached != null)
            {
                _logger.LogInformation(
                    "[EFRepository] Cache HIT: {Key}", KeyAll);
                return cached;
            }

            // Cache MISS — query SQL Server
            _logger.LogInformation(
                "[EFRepository] Cache MISS: {Key} " +
                "→ querying SQL Server", KeyAll);

            var list = _context.QuantityMeasurements
                .OrderByDescending(e => e.Timestamp)
                .ToList();

            // Save to Redis for next request
            SetCache(KeyAll, list);

            return list;
        }

        // ── GetMeasurementsByOperationType → Redis first ──────

        /// <summary>
        /// Returns measurements by operation type.
        /// Redis key: qm:op:COMPARE, qm:op:ADD etc.
        /// UC17, UC18
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetMeasurementsByOperationType(string operationType)
        {
            string key = $"qm:op:{operationType.ToUpper()}";
            var cached = TryGetFromCache
                <List<QuantityMeasurementEntity>>(key);

            if (cached != null)
            {
                _logger.LogInformation(
                    "[EFRepository] Cache HIT: {Key}", key);
                return cached;
            }

            _logger.LogInformation(
                "[EFRepository] Cache MISS: {Key} " +
                "→ querying SQL Server", key);

            var list = _context.QuantityMeasurements
                .Where(e => e.OperationType.ToUpper() ==
                            operationType.ToUpper())
                .OrderByDescending(e => e.Timestamp)
                .ToList();

            SetCache(key, list);
            return list;
        }

        // ── GetMeasurementsByMeasurementType → Redis first ────

        /// <summary>
        /// Returns measurements by measurement type.
        /// Redis key: qm:cat:LENGTH, qm:cat:WEIGHT etc.
        /// UC17, UC18
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetMeasurementsByMeasurementType(string measurementType)
        {
            string key = $"qm:cat:{measurementType.ToUpper()}";
            var cached = TryGetFromCache
                <List<QuantityMeasurementEntity>>(key);

            if (cached != null)
            {
                _logger.LogInformation(
                    "[EFRepository] Cache HIT: {Key}", key);
                return cached;
            }

            _logger.LogInformation(
                "[EFRepository] Cache MISS: {Key} " +
                "→ querying SQL Server", key);

            var list = _context.QuantityMeasurements
                .Where(e => e.MeasurementType != null &&
                    e.MeasurementType.ToUpper() ==
                    measurementType.ToUpper())
                .OrderByDescending(e => e.Timestamp)
                .ToList();

            SetCache(key, list);
            return list;
        }

        // ── Remaining methods (no caching needed) ─────────────

        public QuantityMeasurementEntity? FindById(string id)
            => _context.QuantityMeasurements
                .FirstOrDefault(e => e.Id == id);

        public void DeleteById(string id)
        {
            var entity = _context.QuantityMeasurements
                .FirstOrDefault(e => e.Id == id);
            if (entity == null) return;
            _context.QuantityMeasurements.Remove(entity);
            _context.SaveChanges();
        }

        public void ClearAll()
        {
            _context.QuantityMeasurements
                .RemoveRange(_context.QuantityMeasurements);
            _context.SaveChanges();
            InvalidateCache("ALL", null);
        }

        public int GetTotalCount()
            => _context.QuantityMeasurements.Count();

        public int DeleteAllMeasurements()
        {
            int count = GetTotalCount();
            ClearAll();
            return count;
        }

        public List<QuantityMeasurementEntity> GetErrorMeasurements()
            => _context.QuantityMeasurements
                .Where(e => e.HasError)
                .OrderByDescending(e => e.Timestamp)
                .ToList();

        public int GetCountByOperationType(string operationType)
            => _context.QuantityMeasurements
                .Count(e => e.OperationType.ToUpper() ==
                            operationType.ToUpper());

        public List<QuantityMeasurementEntity>
            GetMeasurementsAfterDate(DateTime date)
            => _context.QuantityMeasurements
                .Where(e => e.Timestamp >= date)
                .OrderByDescending(e => e.Timestamp)
                .ToList();

        public string GetPoolStatistics()
            => $"EFRepository: [Records: {GetTotalCount()}, " +
               $"Provider: {_context.Database.ProviderName}]";

        public void ReleaseResources()
            => _context.Dispose();

        // ── Private Redis Helpers ─────────────────────────────

        /// <summary>
        /// Tries to get data from Redis.
        /// Returns null on miss or Redis failure.
        /// UC18
        /// </summary>
        private T? TryGetFromCache<T>(string key) where T : class
        {
            try
            {
                byte[]? bytes = _cache.Get(key);
                if (bytes == null) return null;
                return JsonSerializer.Deserialize<T>(bytes);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    "[EFRepository] Redis GET failed " +
                    "for key={Key}: {Msg}. " +
                    "Falling back to SQL Server.",
                    key, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Saves data to Redis with 5-min TTL.
        /// Silently continues on Redis failure.
        /// UC18
        /// </summary>
        private void SetCache<T>(string key, T value)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CacheTtl
                };
                _cache.Set(key,
                    JsonSerializer.SerializeToUtf8Bytes(value),
                    options);

                _logger.LogInformation(
                    "[EFRepository] Saved to Redis: {Key} " +
                    "(TTL: 5 min)", key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    "[EFRepository] Redis SET failed " +
                    "for key={Key}: {Msg}.",
                    key, ex.Message);
            }
        }

        /// <summary>
        /// Invalidates Redis cache keys after a write.
        /// Ensures next read gets fresh data from SQL Server.
        /// UC18
        /// </summary>
        private void InvalidateCache(
            string operation, string? category)
        {
            try
            {
                _cache.Remove(KeyAll);
                _cache.Remove(
                    $"qm:op:{operation.ToUpper()}");
                if (!string.IsNullOrEmpty(category))
                    _cache.Remove(
                        $"qm:cat:{category.ToUpper()}");

                _logger.LogInformation(
                    "[EFRepository] Redis cache invalidated " +
                    "after write ✓");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    "[EFRepository] Cache invalidation failed: " +
                    "{Msg}", ex.Message);
            }
        }
    }
}