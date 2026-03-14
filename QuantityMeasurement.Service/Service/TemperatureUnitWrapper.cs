using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurement.Service.Interfaces;

namespace QuantityMeasurement.Service.Service
{
    /// <summary>
    /// Wrapper class for TemperatureUnit to implement IMeasurable.
    /// Uses TemperatureUnitExtensions instance for conversions.
    /// UC14, UC16
    /// </summary>
    public class TemperatureUnitWrapper : IMeasurable
    {
        // ─── Fields ───────────────────────────────────────────

        /// <summary>
        /// Temperature unit this wrapper operates on.
        /// </summary>
        public TemperatureUnit Unit { get; }

        /// <summary>
        /// Instance of extensions for conversion logic.
        /// </summary>
        private readonly TemperatureUnitExtensions _extensions;

        // ─── Constructor ──────────────────────────────────────

        /// <summary>
        /// Initializes wrapper with temperature unit.
        /// Creates TemperatureUnitExtensions instance.
        /// </summary>
        public TemperatureUnitWrapper(TemperatureUnit unit)
        {
            Unit        = unit;
            _extensions = new TemperatureUnitExtensions(unit);
        }

        // ─── IMeasurable Implementation ───────────────────────

        /// <summary>
        /// Converts value to base unit (Celsius).
        /// </summary>
        public double ConvertToBaseUnit(double value)
        {
            return _extensions.ConvertToBaseUnit(value);
        }

        /// <summary>
        /// Converts base unit value to this unit.
        /// </summary>
        public double ConvertFromBaseUnit(double baseValue)
        {
            return _extensions.ConvertFromBaseUnit(baseValue);
        }

        /// <summary>
        /// Returns unit name.
        /// </summary>
        public string GetUnitName()
        {
            return _extensions.GetUnitName();
        }

        /// <summary>
        /// Validates arithmetic operation support.
        /// Temperature throws NotSupportedException.
        /// </summary>
        public void ValidateOperationSupport(string operation)
        {
            _extensions.ValidateOperationSupport(operation);
        }
    }
}