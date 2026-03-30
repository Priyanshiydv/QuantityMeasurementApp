using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using QuantityMeasurement.Models.Entities;
using QuantityMeasurement.Repository.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace QuantityMeasurement.Repository.Service
{
    /// <summary>
    /// Singleton in-memory cache repository for quantity measurements.
    /// Also persists data to disk using JSON serialization.
    /// Updated in UC16 to sync old records to database on startup.
    /// Only keeps today's records in JSON file.
    /// UC15, UC16
    /// </summary>
    public class QuantityMeasurementCacheRepository
        : IQuantityMeasurementRepository
    {
        // ─── Singleton Instance ───────────────────────────────

        private static QuantityMeasurementCacheRepository? _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// Returns the single instance of this repository.
        /// Thread-safe Singleton implementation.
        /// </summary>
        public static QuantityMeasurementCacheRepository GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance =
                            new QuantityMeasurementCacheRepository();
                    }
                }
            }
            return _instance;
        }

        // ─── Fields ───────────────────────────────────────────

        /// <summary>
        /// In-memory cache of today's measurement entities only.
        /// Old records moved to database on startup.
        /// UC15, UC16
        /// </summary>
        private readonly List<QuantityMeasurementEntity> _cache;

        /// <summary>
        /// File path for persisting data to disk.
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        /// Database repository for syncing old records.
        /// Set via SetDatabaseRepository() method.
        /// NULL until SetDatabaseRepository() is called.
        /// UC16
        /// </summary>
        private IQuantityMeasurementRepository? _databaseRepository;

        /// <summary>
        /// Stores old records temporarily until
        /// database repository is set via SetDatabaseRepository().
        /// These records will be synced to database
        /// as soon as database repository is available.
        /// UC16
        /// </summary>
        private readonly List<QuantityMeasurementEntity>
            _pendingSyncRecords;

        /// <summary>
        /// Distributed cache (Redis or Memory fallback).
        /// Set via SetDistributedCache() from DI.
        /// UC18
        /// </summary>
        private IDistributedCache? _distributedCache;

        // ─── Private Constructor (Singleton) ──────────────────

        private QuantityMeasurementCacheRepository()
        {
            _filePath           = "quantity_measurements.json";
            _cache              = new List<QuantityMeasurementEntity>();
            _pendingSyncRecords = new List<QuantityMeasurementEntity>();

            // Load existing data from disk on startup
            // Old records stored in _pendingSyncRecords
            // Today's records stored in _cache
            LoadFromDisk();
        }

        // ─── UC16 Database Sync Setup ─────────────────────────

        /// <summary>
        /// Sets database repository for syncing old records.
        /// Called from NTierMenu when cache repository selected.
        /// Immediately syncs pending old records to database.
        /// UC16
        /// </summary>
        public void SetDatabaseRepository(
            IQuantityMeasurementRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;

            Console.WriteLine(
                "[CacheRepository] Database sync enabled ✓");

            // Sync pending old records now that DB is available
            if (_pendingSyncRecords.Count > 0)
            {
                Console.WriteLine(
                    $"[CacheRepository] Syncing " +
                    $"{_pendingSyncRecords.Count} " +
                    $"pending old records to database...");

                int syncedCount = 0;

                foreach (var entity in _pendingSyncRecords)
                {
                    SyncToDatabase(entity);
                    syncedCount++;
                }

                // Clear pending list after sync
                _pendingSyncRecords.Clear();

                Console.WriteLine(
                    $"[CacheRepository] Successfully synced " +
                    $"{syncedCount} old records to database ✓");
            }
            else
            {
                Console.WriteLine(
                    "[CacheRepository] No pending records to sync.");
            }
        }

        /// <summary>
        /// Sets distributed cache (Redis or Memory fallback).
        /// Called from DependencyInjectionConfig on startup.
        /// UC18
        /// </summary>
        public void SetDistributedCache(IDistributedCache cache)
        {
            _distributedCache = cache;
            Console.WriteLine("[CacheRepository] Distributed cache set ✓");
        }

        // ─── Interface Methods ────────────────────────────────

        /// <summary>
        /// Saves entity to in-memory cache and persists to disk.
        /// UC15, UC16
        /// </summary>
        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _cache.Add(entity);
            SaveToDisk();

            // UC18: Also save to Redis/distributed cache
            if (_distributedCache != null)
            {
                try
                {
                    string key  = $"measurement:{entity.Id}";
                    string json = JsonSerializer.Serialize(entity);

                    _distributedCache.SetString(key, json,
                        new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow =
                                TimeSpan.FromMinutes(30)
                        });

                    Console.WriteLine(
                        $"[CacheRepository] Saved to Redis: {key}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"[CacheRepository] Redis save failed: {ex.Message}");
                }
            }

            Console.WriteLine(
                $"[CacheRepository] Saved entity: {entity.Id}");
        }

        /// <summary>
        /// Returns all saved measurement entities.
        /// Returns copy to prevent external modification.
        /// </summary>
        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            return new List<QuantityMeasurementEntity>(_cache);
        }

        /// <summary>
        /// Finds a measurement entity by its ID.
        /// Returns null if not found.
        /// </summary>
        public QuantityMeasurementEntity? FindById(string id)
        {
            return _cache.Find(e => e.Id == id);
        }

        /// <summary>
        /// Deletes a measurement entity by its ID.
        /// </summary>
        public void DeleteById(string id)
        {
            var entity = FindById(id);

            if (entity != null)
            {
                _cache.Remove(entity);
                SaveToDisk();
            }
        }

        /// <summary>
        /// Clears all stored measurements from cache and disk.
        /// </summary>
        public void ClearAll()
        {
            _cache.Clear();
            SaveToDisk();
        }

        // ─── Disk Persistence ─────────────────────────────────

        /// <summary>
        /// Saves entire cache to disk as JSON.
        /// Only today's records saved to JSON.
        /// Old records removed after sync to database.
        /// UC15, UC16
        /// </summary>
        private void SaveToDisk()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(
                    _cache, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[CacheRepository] Warning: " +
                    $"Could not save to disk. {ex.Message}");
            }
        }

        /// <summary>
        /// Loads existing data from disk into cache.
        /// Separates today's records from old records.
        /// Today's records → _cache (kept in JSON)
        /// Old records → _pendingSyncRecords (waiting for DB)
        /// Old records synced to DB when SetDatabaseRepository() called.
        /// UC15, UC16
        /// </summary>
        private void LoadFromDisk()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);

                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var loaded = JsonSerializer.Deserialize
                            <List<QuantityMeasurementEntity>>(json);

                        if (loaded != null)
                        {
                            // Get today's date for comparison
                            DateTime today = DateTime.Today;

                            foreach (var entity in loaded)
                            {
                                // Check if record is from today
                                if (entity.Timestamp.Date == today)
                                {
                                    // Keep today's records in cache
                                    _cache.Add(entity);
                                }
                                else
                                {
                                    // Old record - store in pending list
                                    // Will sync to DB when
                                    // SetDatabaseRepository() is called
                                    _pendingSyncRecords.Add(entity);
                                }
                            }

                            Console.WriteLine(
                                $"[CacheRepository] Loaded " +
                                $"{_cache.Count} today's records. " +
                                $"{_pendingSyncRecords.Count} old " +
                                $"records pending DB sync.");

                            // Save updated cache to disk
                            // Only today's records remain in JSON
                            // Old records removed from JSON
                            SaveToDisk();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[CacheRepository] Warning: " +
                    $"Could not load from disk. {ex.Message}");
            }
        }

        // ─── UC16 Database Sync Methods ───────────────────────

        /// <summary>
        /// Syncs a single entity to database.
        /// Called for each pending old record.
        /// Skips if database repository not set.
        /// UC16
        /// </summary>
        private void SyncToDatabase(
            QuantityMeasurementEntity entity)
        {
            try
            {
                if (_databaseRepository != null)
                {
                    _databaseRepository.Save(entity);

                    Console.WriteLine(
                        $"[CacheRepository] Synced to DB: " +
                        $"{entity.Id}");
                }
                else
                {
                    Console.WriteLine(
                        $"[CacheRepository] Warning: " +
                        $"Database not set. " +
                        $"Cannot sync record: {entity.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[CacheRepository] Warning: " +
                    $"Could not sync to database. " +
                    $"{ex.Message}");
            }
        }

        // ─── UC16 New Query Methods ───────────────────────────

        /// <summary>
        /// Returns measurements filtered by operation type.
        /// e.g. "COMPARE", "ADD", "SUBTRACT", "DIVIDE", "CONVERT"
        /// Uses LINQ to filter in-memory cache.
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetMeasurementsByOperationType(string operationType)
        {
            return _cache
                .FindAll(entity => entity.OperationType
                    .Equals(operationType,
                             StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns measurements filtered by measurement type.
        /// e.g. "Length", "Weight", "Volume", "Temperature"
        /// Uses LINQ to filter in-memory cache.
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetMeasurementsByMeasurementType(string measurementType)
        {
            return _cache
                .FindAll(entity => entity.MeasurementType != null &&
                    entity.MeasurementType
                        .Equals(measurementType,
                                 StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns total count of measurements in cache.
        /// Useful for monitoring and reporting purposes.
        /// </summary>
        public int GetTotalCount()
        {
            return _cache.Count;
        }

        /// <summary>
        /// Deletes all measurements and returns count deleted.
        /// Same as ClearAll but returns deleted record count.
        /// </summary>
        public int DeleteAllMeasurements()
        {
            int count = _cache.Count;
            ClearAll();
            return count;
        }
        
        /// <summary>
        /// Returns all error measurements from cache.
        /// UC17
        /// </summary>
        public List<QuantityMeasurementEntity> GetErrorMeasurements()
        {
            return _cache.FindAll(entity => entity.HasError);
        }

        /// <summary>
        /// Returns count by operation type from cache.
        /// UC17
        /// </summary>
        public int GetCountByOperationType(string operationType)
        {
            return _cache.FindAll(entity => entity.OperationType
                .Equals(operationType,
                    StringComparison.OrdinalIgnoreCase)).Count;
        }

        /// <summary>
        /// Returns measurements after date from cache.
        /// UC17
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetMeasurementsAfterDate(DateTime date)
        {
            return _cache
                .FindAll(entity => entity.Timestamp >= date);
        }
        
        /// Returns cache statistics.
        /// Shows pending sync count and DB sync status.
        /// UC15, UC16
        /// </summary>
        public string GetPoolStatistics()
        {
            return $"CacheRepository Statistics: " +
                   $"[TotalRecords: {_cache.Count}, " +
                   $"PendingSync: {_pendingSyncRecords.Count}, " +
                   $"FilePath: {_filePath}, " +
                   $"DatabaseSync: " +
                   $"{(_databaseRepository != null ? "Enabled" : "Disabled")}]";
        }

        /// <summary>
        /// Releases cache resources.
        /// Saves final state to disk before releasing.
        /// </summary>
        public void ReleaseResources()
        {
            SaveToDisk();
            Console.WriteLine(
                "[CacheRepository] Resources released.");
        }

        /// <summary>
        /// Retrieves a measurement from Redis by ID.
        /// Returns null if not found in cache.
        /// UC18
        /// </summary>
        public QuantityMeasurementEntity? GetFromCache(string id)
        {
            if (_distributedCache == null) return null;

            try
            {
                string key  = $"measurement:{id}";
                string? json = _distributedCache.GetString(key);

                if (json == null)
                {
                    Console.WriteLine(
                        $"[CacheRepository] Redis MISS: {key}");
                    return null;
                }

                Console.WriteLine(
                    $"[CacheRepository] Redis HIT: {key}");

                return JsonSerializer
                    .Deserialize<QuantityMeasurementEntity>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[CacheRepository] Redis get failed: {ex.Message}");
                return null;
            }
        }

        // ─── ToString ─────────────────────────────────────────

        public override string ToString()
        {
            return $"QuantityMeasurementCacheRepository " +
                   $"[{_cache.Count} records, " +
                   $"Pending: {_pendingSyncRecords.Count}, " +
                   $"DB Sync: " +
                   $"{(_databaseRepository != null ? "ON" : "OFF")}]";
        }
    }
}