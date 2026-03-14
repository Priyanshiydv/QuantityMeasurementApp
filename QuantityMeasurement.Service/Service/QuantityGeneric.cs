using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurement.Models;
using QuantityMeasurement.Service.Interfaces;

namespace QuantityMeasurement.Service.Service
{
    /// <summary>
    /// Generic Quantity class supporting multiple measurement categories.
    /// UC13 - DRY Refactoring with Centralized Arithmetic Logic
    /// UC14
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
        /// Centralized validation logic
        /// </summary>
        private void ValidateArithmeticOperands(QuantityGeneric<U>? other, U? targetUnit, bool targetUnitRequired)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Unit.GetType() != other.Unit.GetType())
                throw new ArgumentException("Cannot perform arithmetic on different measurement categories.");

            if (double.IsNaN(Value) || double.IsInfinity(Value) ||
                double.IsNaN(other.Value) || double.IsInfinity(other.Value))
                throw new ArgumentException("Invalid numeric value.");

            if (targetUnitRequired && targetUnit == null)
                throw new ArgumentException("Target unit cannot be null.");

            // UC14 Temperature restriction
            if (Unit is TemperatureUnitWrapper tempWrapper)
            {
                tempWrapper.ValidateOperationSupport("arithmetic");
            }
        }

        /// <summary>
        /// Core centralized arithmetic logic
        /// </summary>
        private double PerformBaseArithmetic(QuantityGeneric<U> other, ArithmeticOperation operation)
        {
            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);

            switch (operation)
            {
                case ArithmeticOperation.ADD:
                    return base1 + base2;

                case ArithmeticOperation.SUBTRACT:
                    return base1 - base2;

                case ArithmeticOperation.DIVIDE:
                    if (Math.Abs(base2) < EPSILON)
                        throw new ArithmeticException("Division by zero is not allowed.");
                    return base1 / base2;

                default:
                    throw new InvalidOperationException("Unsupported arithmetic operation.");
            }
        }

        /// <summary>
        /// Add quantities (implicit unit)
        /// </summary>
        public QuantityGeneric<U> Add(QuantityGeneric<U>? other)
        {
            if (typeof(U) == typeof(TemperatureUnitWrapper))
        throw new NotSupportedException("Addition not supported for temperature units.");

            ValidateArithmeticOperands(other, Unit, false);

            double resultBase = PerformBaseArithmetic(other!, ArithmeticOperation.ADD);
            double finalValue = Unit.ConvertFromBaseUnit(resultBase);

            return new QuantityGeneric<U>(finalValue, Unit);
        }

        /// <summary>
        /// Add quantities (explicit target unit)
        /// </summary>
        public QuantityGeneric<U> Add(QuantityGeneric<U>? other, U targetUnit)
        {
            if (typeof(U) == typeof(TemperatureUnitWrapper))
        throw new NotSupportedException("Addition not supported for temperature units.");
            ValidateArithmeticOperands(other, targetUnit, true);

            double resultBase = PerformBaseArithmetic(other!, ArithmeticOperation.ADD);
            double finalValue = targetUnit.ConvertFromBaseUnit(resultBase);

            return new QuantityGeneric<U>(finalValue, targetUnit);
        }

        /// <summary>
        /// Subtract quantities (implicit unit)
        /// </summary>
        public QuantityGeneric<U> Subtract(QuantityGeneric<U>? other)
        {
            if (typeof(U) == typeof(TemperatureUnitWrapper))
        throw new NotSupportedException("Addition not supported for temperature units.");

            ValidateArithmeticOperands(other, Unit, false);

            double resultBase = PerformBaseArithmetic(other!, ArithmeticOperation.SUBTRACT);
            double finalValue = Unit.ConvertFromBaseUnit(resultBase);

            finalValue = Math.Round(finalValue, 2);

            return new QuantityGeneric<U>(finalValue, Unit);
        }

        /// <summary>
        /// Subtract quantities (explicit target unit)
        /// </summary>
        public QuantityGeneric<U> Subtract(QuantityGeneric<U>? other, U targetUnit)
        {
            if (typeof(U) == typeof(TemperatureUnitWrapper))
        throw new NotSupportedException("Addition not supported for temperature units.");

            ValidateArithmeticOperands(other, targetUnit, true);

            double resultBase = PerformBaseArithmetic(other!, ArithmeticOperation.SUBTRACT);
            double finalValue = targetUnit.ConvertFromBaseUnit(resultBase);

            finalValue = Math.Round(finalValue, 2);

            return new QuantityGeneric<U>(finalValue, targetUnit);
        }

        /// <summary>
        /// Divide quantities and return scalar
        /// </summary>
        public double Divide(QuantityGeneric<U>? other)
        {
            if (typeof(U) == typeof(TemperatureUnitWrapper))
        throw new NotSupportedException("Addition not supported for temperature units.");
        
            ValidateArithmeticOperands(other, default, false);

            return PerformBaseArithmetic(other!, ArithmeticOperation.DIVIDE);
        }

        /// <summary>
        /// Prevent cross-type subtraction
        /// </summary>
        public QuantityGeneric<U> Subtract<V>(QuantityGeneric<V> other) where V : IMeasurable
        {
            throw new ArgumentException("Cannot subtract quantities of different measurement categories.");
        }

        /// <summary>
        /// Prevent cross-type division
        /// </summary>
        public double Divide<V>(QuantityGeneric<V> other) where V : IMeasurable
        {
            throw new ArgumentException("Cannot divide quantities of different measurement categories.");
        }

        public override string ToString()
        {
            return $"{Value:F2} {Unit.GetUnitName()}";
        }
    }
}