using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using QuantityMeasurement.Models.Entities;
using QuantityMeasurement.Repository.Interfaces;

namespace QuantityMeasurement.Repository.Service
{
    /// <summary>
    /// Singleton in-memory cache repository for quantity measurements.
    /// Also persists data to disk using JSON serialization.
    /// UC15
    /// </summary>
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
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
                        _instance = new QuantityMeasurementCacheRepository();
                    }
                }
            }
            return _instance;
        }

        // ─── Fields ───────────────────────────────────────────

        /// <summary>
        /// In-memory cache of all measurement entities.
        /// </summary>
        private readonly List<QuantityMeasurementEntity> _cache;

        /// <summary>
        /// File path for persisting data to disk.
        /// </summary>
        private readonly string _filePath;

        // ─── Private Constructor (Singleton) ──────────────────

        private QuantityMeasurementCacheRepository()
        {
            _filePath = "quantity_measurements.json";
            _cache    = new List<QuantityMeasurementEntity>();

            // Load existing data from disk on startup
            LoadFromDisk();
        }

        // ─── Interface Methods ────────────────────────────────

        /// <summary>
        /// Saves entity to in-memory cache and persists to disk.
        /// </summary>
        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _cache.Add(entity);
            SaveToDisk();
        }

        /// <summary>
        /// Returns all saved measurement entities.
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
        /// </summary>
        private void SaveToDisk()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_cache, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not save to disk. {ex.Message}");
            }
        }

        /// <summary>
        /// Loads existing data from disk into cache.
        /// Called once during initialization.
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
                            _cache.AddRange(loaded);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not load from disk. {ex.Message}");
            }
        }

        // ─── ToString ─────────────────────────────────────────

        public override string ToString()
        {
            return $"QuantityMeasurementCacheRepository " +
                   $"[{_cache.Count} records]";
        }
    }
}