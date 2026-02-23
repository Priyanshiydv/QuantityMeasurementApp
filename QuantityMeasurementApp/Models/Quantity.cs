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
        private double ConvertToFeet()
        {
            switch (unit)
            {
                case LengthUnit.FEET:
                    return value;

                case LengthUnit.INCHES:
                    return value / 12;

                case LengthUnit.YARDS:
                    return value * 3;

                case LengthUnit.CENTIMETERS:
                    return value * 0.0328084;

                default:
                    throw new ArgumentException("Invalid Unit");
            }
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
            return Math.Abs(this.ConvertToFeet() - other.ConvertToFeet()) < 0.0001;
        }

        /// <summary>
        /// Returns hash code based on base unit value.
        /// </summary>
        public override int GetHashCode()
        {
            return ConvertToFeet().GetHashCode();
        }
    }
}