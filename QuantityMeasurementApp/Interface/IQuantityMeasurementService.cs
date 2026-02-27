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


        // ================= UC9 - WEIGHT OPERATIONS =================

        /// <summary>
        /// Compares two weight quantities for equality.
        /// </summary>
        bool CompareWeight(QuantityWeight weight1, QuantityWeight weight2);

        /// <summary>
        /// Converts given weight to target unit.
        /// </summary>
        QuantityWeight ConvertWeight(QuantityWeight weight, WeightUnit targetUnit);

        /// <summary>
        /// Adds two weight quantities and returns result in first unit.
        /// </summary>
        QuantityWeight AddWeight(QuantityWeight weight1, QuantityWeight weight2);

        /// <summary>
        /// Adds two weight quantities and returns result in specified target unit.
        /// </summary>
        QuantityWeight AddWeight(QuantityWeight weight1, QuantityWeight weight2, WeightUnit targetUnit);
    }
}