using QuantityMeasurementApp.Interfaces;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    /// <summary>
    /// Provides measurement comparison methods.
    /// </summary>
    public class QuantityMeasurementService : IQuantityMeasurementService
    {
        /// <summary>
        /// Compares two Feet objects.
        /// </summary>
        public bool AreEqual(Feet? value1, Feet? value2)
        {
            if (value1 == null || value2 == null)
                return false;

            return value1.Equals(value2);
        }

        /// <summary>
        /// Compares two Inch objects
        /// </summary>
        public bool AreEqual(Inches? value1, Inches? value2)
        {
            if (value1 == null || value2 == null)
                return false;

            return value1.Equals(value2);
        }        

        /// <summary>
        /// Compares two generic Quantity objects (UC3).
        /// </summary>
        /// <param name="value1">First quantity.</param>
        /// <param name="value2">Second quantity.</param>
        /// <returns>True if both quantities are equal after unit conversion.</returns>
        public bool AreEqual(Quantity? value1, Quantity? value2)
        {
            if (value1 == null || value2 == null)
                return false;

            return value1.Equals(value2);
        }

        /// <summary>
        /// UC6: Adds two length quantities (same category - Length).
        /// Result is returned in the unit of the first operand.
        /// </summary>
        public Quantity Add(Quantity? first, Quantity? second)
        {
            // ===================== Validation =====================
            if (first == null || second == null)
                throw new ArgumentNullException("Quantity cannot be null.");

            // Delegating addition logic to Quantity model (DRY + OOP)
            return first.Add(second);
}
    }
}