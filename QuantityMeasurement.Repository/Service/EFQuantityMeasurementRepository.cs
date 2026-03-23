using Microsoft.EntityFrameworkCore;
using QuantityMeasurement.Models.Entities;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Repository.Context;
using Microsoft.Extensions.Logging;

namespace QuantityMeasurement.Repository.Service
{
    /// <summary>
    /// Entity Framework Core implementation of
    /// IQuantityMeasurementRepository.
    /// Replaces manual ADO.NET DatabaseRepository from UC16.
    /// Uses EF Core DbContext for all database operations.
    /// No raw SQL needed - EF Core handles everything!
    /// UC17
    /// </summary>
    public class EFQuantityMeasurementRepository
        : IQuantityMeasurementRepository
    {
        // ─── Fields ───────────────────────────────────────────

        /// <summary>
        /// EF Core DbContext for database operations.
        /// Injected via ASP.NET Core DI Container.
        /// UC17
        /// </summary>
        private readonly QuantityMeasurementDbContext _context;

        /// <summary>
        /// Logger for repository operations.
        /// UC17
        /// </summary>
        private readonly ILogger<EFQuantityMeasurementRepository>
            _logger;

        // ─── Constructor ──────────────────────────────────────

        /// <summary>
        /// Constructor with dependency injection.
        /// DbContext and Logger injected by DI Container.
        /// UC17
        /// </summary>
        public EFQuantityMeasurementRepository(
            QuantityMeasurementDbContext context,
            ILogger<EFQuantityMeasurementRepository> logger)
        {
            _context = context
                ?? throw new ArgumentNullException(
                    nameof(context));
            _logger  = logger;

            _logger.LogInformation(
                "[EFRepository] Initialized ✓");
        }

        // ─── Basic CRUD Operations ────────────────────────────

        /// <summary>
        /// Saves entity to database using EF Core.
        /// No manual SQL needed!
        /// UC17
        /// </summary>
        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(
                    nameof(entity));

            // Add entity to DbContext
            _context.QuantityMeasurements.Add(entity);

            // Save changes to database
            _context.SaveChanges();

            _logger.LogInformation(
                "[EFRepository] Saved entity: {Id}",
                entity.Id);
        }

        /// <summary>
        /// Returns all measurement entities from database.
        /// Uses LINQ to Entities for query.
        /// UC17
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetAllMeasurements()
        {
            // LINQ to Entities query
            List<QuantityMeasurementEntity> measurements =
                _context.QuantityMeasurements
                    .OrderByDescending(e => e.Timestamp)
                    .ToList();

            _logger.LogInformation(
                "[EFRepository] Retrieved {Count} measurements.",
                measurements.Count);

            return measurements;
        }

        /// <summary>
        /// Finds entity by ID using EF Core.
        /// Returns null if not found.
        /// UC17
        /// </summary>
        public QuantityMeasurementEntity? FindById(string id)
        {
            QuantityMeasurementEntity? entity =
                _context.QuantityMeasurements
                    .FirstOrDefault(e => e.Id == id);

            if (entity != null)
                _logger.LogInformation(
                    "[EFRepository] Found entity: {Id}", id);
            else
                _logger.LogWarning(
                    "[EFRepository] Entity not found: {Id}",
                    id);

            return entity;
        }

        /// <summary>
        /// Deletes entity by ID using EF Core.
        /// UC17
        /// </summary>
        public void DeleteById(string id)
        {
            QuantityMeasurementEntity? entity =
                _context.QuantityMeasurements
                    .FirstOrDefault(e => e.Id == id);

            if (entity != null)
            {
                _context.QuantityMeasurements.Remove(entity);
                _context.SaveChanges();

                _logger.LogInformation(
                    "[EFRepository] Deleted entity: {Id}", id);
            }
        }

        /// <summary>
        /// Clears all measurements from database.
        /// UC17
        /// </summary>
        public void ClearAll()
        {
            _context.QuantityMeasurements
                .RemoveRange(
                    _context.QuantityMeasurements);

            _context.SaveChanges();

            _logger.LogInformation(
                "[EFRepository] All measurements cleared.");
        }

        // ─── Query Methods ────────────────────────────────────

        /// <summary>
        /// Returns measurements filtered by operation type.
        /// Uses LINQ to Entities for filtering.
        /// UC17
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetMeasurementsByOperationType(
                string operationType)
        {
            List<QuantityMeasurementEntity> measurements =
                _context.QuantityMeasurements
                    .Where(e => e.OperationType.ToUpper()
                        == operationType.ToUpper())
                    .OrderByDescending(e => e.Timestamp)
                    .ToList();

            _logger.LogInformation(
                "[EFRepository] Found {Count} measurements " +
                "for operation: {OperationType}",
                measurements.Count,
                operationType);

            return measurements;
        }

        /// <summary>
        /// Returns measurements filtered by measurement type.
        /// Uses LINQ to Entities for filtering.
        /// UC17
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetMeasurementsByMeasurementType(
                string measurementType)
        {
            List<QuantityMeasurementEntity> measurements =
                _context.QuantityMeasurements
                    .Where(e => e.MeasurementType != null &&
                        e.MeasurementType.ToUpper() ==
                        measurementType.ToUpper())
                    .OrderByDescending(e => e.Timestamp)
                    .ToList();

            _logger.LogInformation(
                "[EFRepository] Found {Count} measurements " +
                "for type: {MeasurementType}",
                measurements.Count,
                measurementType);

            return measurements;
        }

        /// <summary>
        /// Returns total count of measurements.
        /// UC17
        /// </summary>
        public int GetTotalCount()
        {
            return _context.QuantityMeasurements.Count();
        }

        /// <summary>
        /// Deletes all measurements and returns count.
        /// UC17
        /// </summary>
        public int DeleteAllMeasurements()
        {
            int count =
                _context.QuantityMeasurements.Count();
            ClearAll();

            _logger.LogInformation(
                "[EFRepository] Deleted {Count} measurements.",
                count);

            return count;
        }

        /// <summary>
        /// Returns all error measurements.
        /// UC17
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetErrorMeasurements()
        {
            return _context.QuantityMeasurements
                .Where(e => e.HasError)
                .OrderByDescending(e => e.Timestamp)
                .ToList();
        }

        /// <summary>
        /// Returns count by operation type.
        /// UC17
        /// </summary>
        public int GetCountByOperationType(
            string operationType)
        {
            return _context.QuantityMeasurements
                .Count(e => e.OperationType.ToUpper() ==
                    operationType.ToUpper());
        }

        /// <summary>
        /// Returns measurements after specific date.
        /// UC17
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetMeasurementsAfterDate(DateTime date)
        {
            return _context.QuantityMeasurements
                .Where(e => e.Timestamp >= date)
                .OrderByDescending(e => e.Timestamp)
                .ToList();
        }

        // ─── Default Interface Methods ────────────────────────

        /// <summary>
        /// Returns EF Core statistics.
        /// UC17
        /// </summary>
        public string GetPoolStatistics()
        {
            return $"EFRepository: " +
                   $"[TotalRecords: {GetTotalCount()}, " +
                   $"Provider: " +
                   $"{_context.Database.ProviderName}]";
        }

        /// <summary>
        /// Releases EF Core resources.
        /// UC17
        /// </summary>
        public void ReleaseResources()
        {
            _context.Dispose();
            _logger.LogInformation(
                "[EFRepository] Resources released.");
        }

        // ─── ToString ─────────────────────────────────────────

        public override string ToString()
        {
            return $"EFQuantityMeasurementRepository " +
                   $"[Records: {GetTotalCount()}]";
        }
    }
}