using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Generic Quantity class supporting multiple measurement categories.
    /// UC10 - Generic Refactor
    /// </summary>
    public class QuantityGeneric<U> where U : IMeasurable
    {
        private const double EPSILON = 0.0001;

        public double Value { get; }
        public U Unit { get; }

        /// <summary>
        /// Constructor validation
        /// </summary>
        public QuantityGeneric(double value, U unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit));

            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Equality check with cross-category prevention
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not QuantityGeneric<U> other)
                return false;

            // Prevent cross-category comparison
            if (Unit.GetType() != other.Unit.GetType())
                return false;

            double thisBase = Unit.ConvertToBaseUnit(Value);
            double otherBase = other.Unit.ConvertToBaseUnit(other.Value);

            return Math.Abs(thisBase - otherBase) < EPSILON;
        }

        public override int GetHashCode()
        {
            double baseValue = Unit.ConvertToBaseUnit(Value);
            return baseValue.GetHashCode();
        }

        /// <summary>
        /// Convert to target unit
        /// </summary>
        public QuantityGeneric<U> ConvertTo(U targetUnit)
        {
            if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null.");
                
            double baseValue = Unit.ConvertToBaseUnit(Value);
            double converted = targetUnit.ConvertFromBaseUnit(baseValue);

            return new QuantityGeneric<U>(converted, targetUnit);
        }

        /// <summary>
        /// Add and return result in first operand unit
        /// </summary>
        public QuantityGeneric<U> Add(QuantityGeneric<U> other)
        {
            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);

            double sumBase = base1 + base2;
            double finalValue = Unit.ConvertFromBaseUnit(sumBase);

            return new QuantityGeneric<U>(finalValue, Unit);
        }

        /// <summary>
        /// Add and return result in specified target unit
        /// </summary>
        public QuantityGeneric<U> Add(QuantityGeneric<U> other, U targetUnit)
        {
             if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null.");
            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);

            double sumBase = base1 + base2;
            double finalValue = targetUnit.ConvertFromBaseUnit(sumBase);

            return new QuantityGeneric<U>(finalValue, targetUnit);
        }

        public override string ToString()
        {
            return $"{Value:F2} {Unit.GetUnitName()}";
        }
    }
}