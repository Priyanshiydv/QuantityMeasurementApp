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

        /// <summary>
        /// UC6: Adds two length quantities and returns result 
        /// in the unit of the first operand.
        /// </summary>
        /// <param name="first">First quantity.</param>
        /// <param name="second">Second quantity.</param>
        /// <returns>New Quantity with summed value in first operand unit.</returns>
        Quantity Add(Quantity first, Quantity second);

        /// <summary>
        /// UC7: Adds two quantities and returns result in explicit target unit.
        /// </summary>
        /// <param name="first">First quantity</param>
        /// <param name="second">Second quantity</param>
        /// <param name="targetUnit">Explicit target unit for result</param>
        /// <returns>New Quantity in target unit</returns>
        Quantity Add(Quantity first, Quantity second, LengthUnit targetUnit);
    }
}