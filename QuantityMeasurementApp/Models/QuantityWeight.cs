using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents a weight quantity with value and unit.
    /// Supports equality, conversion and addition.
    /// </summary>
    public class QuantityWeight
    {
        private const double EPSILON = 0.0001;

        /// <summary>
        /// Gets the numeric value of the weight.
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Gets the weight unit.
        /// </summary>
        public WeightUnit Unit { get; }

        /// <summary>
        /// Initializes a new instance of weight quantity.
        /// </summary>
        public QuantityWeight(double value, WeightUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Converts current weight to target unit.
        /// </summary>
        public QuantityWeight ConvertTo(WeightUnit targetUnit)
        {
            double baseValue = Unit.ConvertToBaseUnit(Value);
            double convertedValue = targetUnit.ConvertFromBaseUnit(baseValue);

            return new QuantityWeight(convertedValue, targetUnit);
        }

        /// <summary>
        /// Adds two weight quantities and returns result in first quantity's unit.
        /// </summary>
        public QuantityWeight Add(QuantityWeight other)
        {
            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);

            double sumBase = base1 + base2;

            double finalValue = Unit.ConvertFromBaseUnit(sumBase);

            return new QuantityWeight(finalValue, Unit);
        }

        /// <summary>
        /// Adds two weight quantities and returns result in specified target unit.
        /// </summary>
        public QuantityWeight Add(QuantityWeight other, WeightUnit targetUnit)
        {
            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);

            double sumBase = base1 + base2;

            double finalValue = targetUnit.ConvertFromBaseUnit(sumBase);

            return new QuantityWeight(finalValue, targetUnit);
        }

        /// <summary>
        /// Determines whether two weight quantities are equal after conversion to base unit.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            QuantityWeight other = (QuantityWeight)obj;

            double thisInKg = Unit.ConvertToBaseUnit(Value);
            double otherInKg = other.Unit.ConvertToBaseUnit(other.Value);

            return Math.Abs(thisInKg - otherInKg) < EPSILON;
        }

        /// <summary>
        /// Returns hash code based on base unit value.
        /// </summary>
        public override int GetHashCode()
        {
            return Unit.ConvertToBaseUnit(Value).GetHashCode();
        }

    }
}