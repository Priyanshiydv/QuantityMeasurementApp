using System.Collections.Generic;
using QuantityMeasurement.Models.Entities;

namespace QuantityMeasurement.Repository.Interfaces
{
    /// <summary>
    /// Repository interface for quantity measurement data access.
    /// Follows Interface Segregation Principle.
    /// Allows easy substitution of different repository implementations.
    /// UC15
    /// </summary>
    public interface IQuantityMeasurementRepository
    {
        /// <summary>
        /// Saves a quantity measurement entity to the repository.
        /// </summary>
        void Save(QuantityMeasurementEntity entity);

        /// <summary>
        /// Returns all saved measurement entities.
        /// </summary>
        List<QuantityMeasurementEntity> GetAllMeasurements();

        /// <summary>
        /// Finds a measurement entity by its ID.
        /// </summary>
        QuantityMeasurementEntity? FindById(string id);

        /// <summary>
        /// Deletes a measurement entity by its ID.
        /// </summary>
        void DeleteById(string id);

        /// <summary>
        /// Clears all stored measurements.
        /// </summary>
        void ClearAll();

        // ─── UC16 New Query Methods ───────────────────────────

        /// <summary>
        /// Returns all measurements filtered by operation type.
        /// e.g. "COMPARE", "ADD", "SUBTRACT", "DIVIDE", "CONVERT"
        /// </summary>
        List<QuantityMeasurementEntity> GetMeasurementsByOperationType(
            string operationType);

        /// <summary>
        /// Returns all measurements filtered by measurement type.
        /// e.g. "Length", "Weight", "Volume", "Temperature"
        /// </summary>
        List<QuantityMeasurementEntity> GetMeasurementsByMeasurementType(
            string measurementType);

        /// <summary>
        /// Returns total count of measurements in repository.
        /// Useful for monitoring and reporting purposes.
        /// </summary>
        int GetTotalCount();

        /// <summary>
        /// Deletes all measurements from repository.
        /// Same as ClearAll but returns count of deleted records.
        /// </summary>
        int DeleteAllMeasurements();
        
        /// <summary>
        /// Returns all error measurements.
        /// Added in UC17.
        /// </summary>
        List<QuantityMeasurementEntity> GetErrorMeasurements();

        /// <summary>
        /// Returns count by operation type.
        /// Added in UC17.
        /// </summary>
        int GetCountByOperationType(string operationType);

        /// <summary>
        /// Returns measurements after specific date.
        /// Added in UC17.
        /// </summary>
        List<QuantityMeasurementEntity> GetMeasurementsAfterDate(
            DateTime date);

        /// <summary>
        /// Returns measurements for a specific user.
        /// UC19
        /// </summary>
        List<QuantityMeasurementEntity> GetMeasurementsByUserId(int userId);

        // ─── UC16 Default Methods ─────────────────────────────

        /// <summary>
        /// Returns pool statistics for database repositories.
        /// Default implementation returns basic info.
        /// Override in database repository for detailed stats.
        /// </summary>
        string GetPoolStatistics()
        {
            return "Pool statistics not available " +
                   "for this repository type.";
        }

        /// <summary>
        /// Releases all resources held by the repository.
        /// Called when application is shutting down.
        /// Default implementation does nothing.
        /// Override in database repository to close connections.
        /// </summary>
        void ReleaseResources()
        {
            // Default: do nothing
            // Database repository will override this
        }
    }
}