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
    }
}