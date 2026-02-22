using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents a generic quantity with value and unit (UC3 - DRY Principle).
    /// </summary>
    public class Quantity
    {
        private readonly double value;
        private readonly LengthUnit unit;

        /// <summary>
        /// Initializes Quantity object with value and unit.
        /// </summary>
        /// <param name="value">The numerical measurement value.</param>
        /// <param name="unit">The unit of measurement (Feet or Inch).</param>
        public Quantity(double value, LengthUnit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        /// <summary>
        /// Converts the quantity to base unit (Feet).
        /// </summary>
        /// <returns>Value converted to feet.</returns>
        private double ToFeet()
        {
            return unit switch
            {
                LengthUnit.Feet => value,
                LengthUnit.Inch => value / 12.0,
                _ => throw new InvalidOperationException("Unsupported unit")
            };
        }

        /// <summary>
        /// Checks equality between two Quantity objects using unit conversion.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if quantities are equal; otherwise false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Quantity))
                return false;

            Quantity other = (Quantity)obj;

            // Convert both to base unit (Feet) and compare
            return Math.Abs(this.ToFeet() - other.ToFeet()) < 0.0001;
        }

        /// <summary>
        /// Returns hash code based on base unit value.
        /// </summary>
        public override int GetHashCode()
        {
            return ToFeet().GetHashCode();
        }
    }
}