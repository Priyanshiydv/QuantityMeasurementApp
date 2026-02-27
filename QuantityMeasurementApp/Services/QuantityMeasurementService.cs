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

            // Delegating addition logic to Quantity model 
            return first.Add(second); //result in first operand unit
        }

        /// <summary>
        /// UC7: Adds two length quantities and returns result in EXPLICIT target unit.
        /// </summary>
        public Quantity Add(Quantity? first, Quantity? second, LengthUnit targetUnit)
        {
            // ===================== Validation =====================
            if (first == null || second == null)
                throw new ArgumentNullException("Quantity cannot be null.");

            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
                throw new ArgumentException("Invalid Target Unit");

            // Delegates to Model (OOP + DRY)
            return first.Add(second, targetUnit); //result in target unit
        }

//======================================UC9==================================================
        /// <summary>
        /// Compares two weight quantities for equality.
        /// </summary>
        public bool CompareWeight(QuantityWeight weight1, QuantityWeight weight2)
        {
            if (weight1 == null || weight2 == null)
                throw new ArgumentNullException("Weight cannot be null");

            return weight1.Equals(weight2);
        }
        /// <summary>
        /// Converts given weight to target unit.
        /// </summary>
        public QuantityWeight ConvertWeight(QuantityWeight weight, WeightUnit targetUnit)
        {
            if (weight == null)
                throw new ArgumentNullException("Weight cannot be null");

            return weight.ConvertTo(targetUnit);
        }

        /// <summary>
        /// Adds two weight quantities and returns result in first unit.
        /// </summary>
        public QuantityWeight AddWeight(QuantityWeight weight1, QuantityWeight weight2)
        {
            if (weight1 == null || weight2 == null)
                throw new ArgumentNullException("Weight cannot be null");

            return weight1.Add(weight2);
        }

        /// <summary>
        /// Adds two weight quantities and returns result in specified target unit.
        /// </summary>
        public QuantityWeight AddWeight(QuantityWeight weight1, QuantityWeight weight2, WeightUnit targetUnit)
        {
            if (weight1 == null || weight2 == null)
                throw new ArgumentNullException("Weight cannot be null");

            return weight1.Add(weight2, targetUnit);
        }
    }
}