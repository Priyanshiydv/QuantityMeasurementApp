using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents supported volume units.
    /// Implements IMeasurable for UC11.
    /// Base unit = Litre
    /// </summary>
    public class VolumeUnit : IMeasurable
    {
        private readonly string name;
        private readonly double toLitreFactor;

        /// <summary>
        /// Private constructor to restrict object creation.
        /// Units are exposed as static constants.
        /// </summary>
        private VolumeUnit(string name, double toLitreFactor)
        {
            this.name = name;
            this.toLitreFactor = toLitreFactor;
        }

        // ---------------- PREDEFINED UNITS ----------------

        /// <summary>
        /// Base unit for volume.
        /// </summary>
        public static readonly VolumeUnit LITRE =
            new VolumeUnit("LITRE", 1);

        /// <summary>
        /// 1 mL = 0.001 L
        /// </summary>
        public static readonly VolumeUnit MILLILITRE =
            new VolumeUnit("MILLILITRE", 0.001);

        /// <summary>
        /// 1 Gallon ≈ 3.78541 L
        /// </summary>
        public static readonly VolumeUnit GALLON =
            new VolumeUnit("GALLON", 3.78541);

        // ---------------- CONVERSION METHODS ----------------

        /// <summary>
        /// Converts given value to base unit (Litre).
        /// </summary>
        public double ConvertToBaseUnit(double value)
        {
            return value * toLitreFactor;
        }

        /// <summary>
        /// Converts base unit (Litre) value to this unit.
        /// </summary>
        public double ConvertFromBaseUnit(double baseValue)
        {
            return baseValue / toLitreFactor;
        }

        /// <summary>
        /// Returns readable unit name.
        /// </summary>
        public string GetUnitName()
        {
            return name;
        }
    }
}