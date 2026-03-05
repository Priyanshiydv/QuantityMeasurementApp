using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents supported length units.
    /// Implements IMeasurable for UC10.
    /// </summary>
    public class LengthUnit : IMeasurable
    {
        private readonly string name;
        private readonly double toFeetFactor;

        private LengthUnit(string name, double toFeetFactor)
        {
            this.name = name;
            this.toFeetFactor = toFeetFactor;
        }

        // Static predefined units
        public static readonly LengthUnit FEET = new LengthUnit("FEET", 1);
        public static readonly LengthUnit INCHES = new LengthUnit("INCHES", 1.0 / 12);
        public static readonly LengthUnit YARDS = new LengthUnit("YARDS", 3);
        public static readonly LengthUnit CENTIMETERS = new LengthUnit("CENTIMETERS", 1.0 / 30.48);

        public double ConvertToBaseUnit(double value)
        {
            return value * toFeetFactor;
        }

        public double ConvertFromBaseUnit(double baseValue)
        {
            return baseValue / toFeetFactor;
        }

        public string GetUnitName()
        {
            return name;
        }

        public double ToBase(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            return ConvertToBaseUnit(value);
        }

        public double FromBase(double baseValue)
        {
            if (double.IsNaN(baseValue) || double.IsInfinity(baseValue))
                throw new ArgumentException("Invalid numeric value");

            return ConvertFromBaseUnit(baseValue);
        }
    }
}