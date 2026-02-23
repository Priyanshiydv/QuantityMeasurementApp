using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Interfaces
{
    /// <summary>
    /// Defines comparison operations for quantity measurements.
    /// </summary>
    public interface IQuantityMeasurementService
    {
        /// <summary>
        /// Compares two Feet objects.
        /// </summary>
        /// <param name="first">First Feet object.</param>
        /// <param name="second">Second Feet object.</param>
        /// <returns>True if equal; otherwise false.</returns>
        bool AreEqual(Feet first, Feet second);

        /// <summary>
        /// Compares two Inches objects.
        /// </summary>
        /// <param name="first">First Inches object.</param>
        /// <param name="second">Second Inches object.</param>
        /// <returns>True if equal; otherwise false.</returns>
        bool AreEqual(Inches first, Inches second);

        /// <summary>
        /// Compares two generic Quantity objects.
        /// </summary>
        /// <param name="first">First Quantity.</param>
        /// <param name="second">Second Quantity.</param>
        /// <returns>True if equal; otherwise false.</returns>
        bool AreEqual(Quantity first, Quantity second);
    }
}