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
        /// Gets the numerical value of the quantity.
        /// </summary>
        public double Value
        {
            get { return value; }
        }

        /// <summary>
        /// Gets the unit of the quantity.
        /// </summary>
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
        /// Checks equality between two Quantity objects using unit conversion.
        /// UC1 & UC2 - Equality with unit conversion
        /// UC8 - Uses LengthUnit extension methods (SRP applied)
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if quantities are equal; otherwise false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Quantity))
                return false;

            Quantity other = (Quantity)obj;
        // ===================== Convert both to Base Unit (Feet) =====================
            double thisBaseValue = this.unit.ToBase(this.value);
            double otherBaseValue = other.unit.ToBase(other.value);

        // ===================== Floating Point Tolerance Comparison =====================
            return Math.Abs(thisBaseValue - otherBaseValue) < 0.0001;
        }

        /// <summary>
        /// Returns hash code based on base unit value.
        /// UC1 Requirement - Consistent hashing with equality
        /// UC8 - Uses LengthUnit extension conversion
        /// </summary>
        public override int GetHashCode()
        {
             double baseValue = unit.ToBase(value);
            return baseValue.GetHashCode();
        }

        /// <summary>
        /// Converts a numeric value from source unit to target unit.
        /// UC4 - Unit Conversion Feature
        /// UC8 - Refactored: Delegates conversion to LengthUnitExtensions (SRP)
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
        // ===================== Step 1: Convert Source to Base Unit (Feet) =====================
            double baseValue = source.ToBase(value);

       // ===================== Step 2: Convert Base Unit to Target Unit =====================
            double convertedValue = target.FromBase(baseValue);

            return convertedValue;
        }

        /// <summary>
        /// Converts current Quantity to a specified target unit.
        /// UC4 - Instance Convert API (Better Usability)
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