using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    /// <summary>
    /// Provides measurement comparison methods.
    /// </summary>
    public class QuantityMeasurementService
    {
        /// <summary>
        /// Compares two Feet objects.
        /// </summary>
        /// <param name="value1">First Feet measurement.</param>
        /// <param name="value2">Second Feet measurement.</param>
        /// <returns>True if both values are equal; otherwise false.</returns>
        public bool AreEqual(Feet? value1, Feet? value2)
        {
            if (value1 == null || value2 == null)
                return false;

            return value1.Equals(value2);
        }

        /// <summary>
        /// Compares two Inches objects.
        /// </summary>
        /// <param name="value1">First Inches measurement.</param>
        /// <param name="value2">Second Inches measurement.</param>
        /// <returns>True if both values are equal; otherwise false.</returns>
        public bool AreEqual(Inches? value1, Inches? value2)
        {
            if (value1 == null || value2 == null)
                return false;

            return value1.Equals(value2);
        }

        /// <summary>
        /// Compares two Feet values using double input (used in Menu).
        /// </summary>
        public static bool CompareFeet(double value1, double value2)
        {
            Feet feet1 = new Feet(value1);
            Feet feet2 = new Feet(value2);

            return feet1.Equals(feet2);
        }

        /// <summary>
        /// Compares two Inches values using double input
        /// </summary>
        public static bool CompareInches(double value1, double value2)
        {
            Inches inch1 = new Inches(value1);
            Inches inch2 = new Inches(value2);

            return inch1.Equals(inch2);
        }
    }
}