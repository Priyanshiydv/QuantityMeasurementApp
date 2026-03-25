using System;
using QuantityMeasurementApp.Enums;
using QuantityMeasurement.Service.Interfaces;

namespace QuantityMeasurement.Service.Service
{
    /// <summary>
    /// Provides temperature unit conversion operations.
    /// Converted from static extension class to instance class
    /// in UC16 refactoring as per teacher feedback.
    /// UC14, UC16
    /// </summary>
    public class TemperatureUnitExtensions
    {
        // ─── Fields ───────────────────────────────────────────

        /// <summary>
        /// Temperature unit this instance operates on.
        /// </summary>
        private readonly TemperatureUnit _unit;

        // ─── Constructor ──────────────────────────────────────

        /// <summary>
        /// Initializes with specific temperature unit.
        /// </summary>
        public TemperatureUnitExtensions(TemperatureUnit unit)
        {
            _unit = unit;
        }

        // ─── Arithmetic Support ───────────────────────────────

        /// <summary>
        /// Delegate indicating temperature does NOT
        /// support arithmetic operations.
        /// </summary>
        public ArithmeticSupportDelegate GetArithmeticSupport()
            => () => false;

        // ─── Conversion Methods ───────────────────────────────

        /// <summary>
        /// Converts temperature value to base unit (Celsius).
        /// </summary>
        public double ConvertToBaseUnit(double value)
        {
            return _unit switch
            {
                TemperatureUnit.CELSIUS    => value,
                TemperatureUnit.FAHRENHEIT => (value - 32) * 5.0 / 9,
                TemperatureUnit.KELVIN     => value - 273.15,
                _ => throw new ArgumentException(
                    "Invalid temperature unit")
            };
        }

        /// <summary>
        /// Converts Celsius base value to target temperature unit.
        /// </summary>
        public double ConvertFromBaseUnit(double baseValue)
        {
            return _unit switch
            {
                TemperatureUnit.CELSIUS    => baseValue,
                TemperatureUnit.FAHRENHEIT => (baseValue * 9.0 / 5) + 32,
                TemperatureUnit.KELVIN     => baseValue + 273.15,
                _ => throw new ArgumentException(
                    "Invalid temperature unit")
            };
        }

        /// <summary>
        /// Returns string name of temperature unit.
        /// </summary>
        public string GetUnitName()
        {
            return _unit.ToString();
        }

        /// <summary>
        /// Validates that operation is supported.
        /// Temperature does NOT support arithmetic operations.
        /// Throws NotSupportedException always.
        /// </summary>
        public void ValidateOperationSupport(string operation)
        {
            throw new NotSupportedException(
                $"Temperature does not support " +
                $"{operation} operation.");
        }
    }
}