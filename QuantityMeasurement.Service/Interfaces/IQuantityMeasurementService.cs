using QuantityMeasurement.Models.UnitDTOs;

namespace QuantityMeasurement.Service.Interfaces
{
    /// <summary>
    /// Service interface for quantity measurement operations.
    /// Accepts QuantityDTO as input and returns QuantityDTO as output.
    /// UC15
    /// </summary>
    public interface IQuantityMeasurementService
    {
        /// <summary>
        /// Compares two quantities for equality.
        /// Returns QuantityDTO with Result = "true" or "false"
        /// </summary>
        QuantityDTO Compare(QuantityDTO first, QuantityDTO second, int? userId = null);

        /// <summary>
        /// Converts a quantity from one unit to another.
        /// Returns QuantityDTO with converted value and target unit.
        /// </summary>
        QuantityDTO Convert(QuantityDTO quantity, string targetUnit, int? userId = null);

        /// <summary>
        /// Adds two quantities together.
        /// Returns QuantityDTO with result in first quantity's unit.
        /// </summary>
        QuantityDTO Add(QuantityDTO first, QuantityDTO second, int? userId = null);

        /// <summary>
        /// Adds two quantities together in specified target unit.
        /// Returns QuantityDTO with result in target unit.
        /// </summary>
        QuantityDTO Add(QuantityDTO first, QuantityDTO second, string targetUnit, int? userId = null);

        /// <summary>
        /// Subtracts second quantity from first.
        /// Returns QuantityDTO with result in first quantity's unit.
        /// </summary>
        QuantityDTO Subtract(QuantityDTO first, QuantityDTO second, int? userId = null);

        /// <summary>
        /// Divides first quantity by second.
        /// Returns QuantityDTO with scalar result (no unit).
        /// </summary>
        QuantityDTO Divide(QuantityDTO first, QuantityDTO second, int? userId = null);
    }
}