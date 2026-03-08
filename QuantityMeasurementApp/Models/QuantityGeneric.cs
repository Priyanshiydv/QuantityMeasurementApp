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



        /// <summary>
        /// Subtract and return result in first operand unit (implicit target)
        /// </summary>
        public QuantityGeneric<U> Subtract(QuantityGeneric<U>? other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);

            double diffBase = base1 - base2;
            double finalValue = Unit.ConvertFromBaseUnit(diffBase);

            finalValue = Math.Round(finalValue, 2);

            return new QuantityGeneric<U>(finalValue, Unit);
        }

        /// <summary>
        /// Subtract and return result in specified target unit
        /// </summary>
        public QuantityGeneric<U> Subtract(QuantityGeneric<U> other, U targetUnit)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null.");

            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);

            double diffBase = base1 - base2;
            double finalValue = targetUnit.ConvertFromBaseUnit(diffBase);

            finalValue = Math.Round(finalValue, 2);

            return new QuantityGeneric<U>(finalValue, targetUnit);
        }

        /// <summary>
        /// Divide two quantities and return dimensionless ratio
        /// </summary>
        public double Divide(QuantityGeneric<U> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);

            if (Math.Abs(base2) < EPSILON)
                throw new ArithmeticException("Division by zero is not allowed.");

            double result = base1 / base2;

            return result;
        }

        // Cross-type subtraction
        public QuantityGeneric<U> Subtract<V>(QuantityGeneric<V> other) where V : IMeasurable
        {
            throw new ArgumentException("Cannot subtract quantities of different measurement categories.");
        }

        // Cross-type division
        public double Divide<V>(QuantityGeneric<V> other) where V : IMeasurable
        {
            throw new ArgumentException("Cannot divide quantities of different measurement categories.");
        }
    }
}