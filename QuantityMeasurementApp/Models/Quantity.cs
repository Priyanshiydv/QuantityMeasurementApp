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

        public double Value
        {
            get { return value; }
        }

        public LengthUnit Unit
        {
            get { return unit; }
        }

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

        /// <summary>
        /// Converts a numeric value from source unit to target unit.
        /// </summary>
        /// <param name="value">Numeric measurement value.</param>
        /// <param name="source">Source measurement unit.</param>
        /// <param name="target">Target measurement unit.</param>
        /// <returns>Converted value in target unit.</returns>
        /// static convert
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            // ===================== Input Validation =====================
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Value must be a finite number.");
            }

            if (!Enum.IsDefined(typeof(LengthUnit), source))
            {
                throw new ArgumentException("Invalid Source Unit");
            }

            if (!Enum.IsDefined(typeof(LengthUnit), target))
            {
                throw new ArgumentException("Invalid Target Unit");
            }

            // ===================== Step 1: Convert source to Feet =====================
            double valueInFeet = source switch
            {
                LengthUnit.FEET => value,
                LengthUnit.INCHES => value / 12,
                LengthUnit.YARDS => value * 3,
                LengthUnit.CENTIMETERS => value / 30.48,
                _ => throw new ArgumentException("Invalid Source Unit") // safety
            };

            // ===================== Step 2: Convert Feet to target unit =====================
            return target switch
            {
                LengthUnit.FEET => valueInFeet,
                LengthUnit.INCHES => valueInFeet * 12,
                LengthUnit.YARDS => valueInFeet / 3,
                LengthUnit.CENTIMETERS => valueInFeet * 30.48,
                _ => throw new ArgumentException("Invalid Target Unit") // safety
            };
        }

        /// <summary>
        /// Converts current Quantity to a specified target unit.
        /// </summary>
        /// <param name="targetUnit">Unit to convert into.</param>
        /// <returns>New Quantity object with converted value.</returns>
        /// Instance ConvertTo API Usability
        public Quantity ConvertTo(LengthUnit targetUnit)
        {
            double convertedValue = Convert(this.value, this.unit, targetUnit);
            return new Quantity(convertedValue, targetUnit);
        }

        /// <summary>
        /// Returns readable string representation of Quantity.
        /// </summary>
        /// <returns>Formatted value with unit.</returns>
        public override string ToString()
        {
            return $"{value:F2} {unit}";
        }

        /// <summary>
        /// UC6: Adds another Quantity to current Quantity.
        /// Result is returned in the unit of the first operand (this.unit).
        /// </summary>
        /// <param name="other">Second quantity to add.</param>
        /// <returns>New Quantity object with summed value in first operand unit.</returns>
        public Quantity Add(Quantity other)
        {
            // ===================== Validation =====================
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (double.IsNaN(this.value) || double.IsInfinity(this.value) ||
                double.IsNaN(other.value) || double.IsInfinity(other.value))
            {
                throw new ArgumentException("Values must be finite numbers.");
            }

            // ===================== Step 1: Convert both to Base Unit (Feet) =====================
            double firstInFeet = Convert(this.value, this.unit, LengthUnit.FEET);
            double secondInFeet = Convert(other.value, other.unit, LengthUnit.FEET);

            // ===================== Step 2: Add in Base Unit =====================
            double sumInFeet = firstInFeet + secondInFeet;

            // ===================== Step 3: Convert back to First Operand Unit =====================
            double resultValue = Convert(sumInFeet, LengthUnit.FEET, this.unit);

            // ===================== Step 4: Return New Immutable Object =====================
            return new Quantity(resultValue, this.unit);
        }

        /// <summary>
        /// UC7: Adds two quantities and returns result in EXPLICIT target unit.
        /// Method Overloading 
        /// </summary>
        /// <param name="other">Second quantity to add.</param>
        /// <param name="targetUnit">Explicit target unit for result.</param>
        /// <returns>New Quantity in target unit.</returns>
        public Quantity Add(Quantity other, LengthUnit targetUnit)
        {
            // ===================== Validation =====================
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
                throw new ArgumentException("Invalid Target Unit");

            if (double.IsNaN(this.value) || double.IsInfinity(this.value) ||
                double.IsNaN(other.value) || double.IsInfinity(other.value))
            {
                throw new ArgumentException("Values must be finite numbers.");
            }

            // ===================== Step 1: Convert both to Base Unit (Feet) =====================
            double firstInFeet = Convert(this.value, this.unit, LengthUnit.FEET);
            double secondInFeet = Convert(other.value, other.unit, LengthUnit.FEET);

            // ===================== Step 2: Add in Base Unit =====================
            double sumInFeet = firstInFeet + secondInFeet;

            // ===================== Step 3: Convert to EXPLICIT Target Unit =====================
            double resultValue = Convert(sumInFeet, LengthUnit.FEET, targetUnit);

            // ===================== Step 4: Return New Immutable Object =====================
            return new Quantity(resultValue, targetUnit);
        }
    }
}