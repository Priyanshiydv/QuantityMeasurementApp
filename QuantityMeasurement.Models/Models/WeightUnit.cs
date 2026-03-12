using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents supported weight units.
    /// Implements IMeasurable for UC10.
    /// Base unit = Kilogram
    /// </summary>
    public class WeightUnit : IMeasurable
    {
        private readonly string name;
        private readonly double toKgFactor;

        private WeightUnit(string name, double toKgFactor)
        {
            this.name = name;
            this.toKgFactor = toKgFactor;
        }

        // Predefined Units
        public static readonly WeightUnit KILOGRAM = new WeightUnit("KILOGRAM", 1);
        public static readonly WeightUnit GRAM = new WeightUnit("GRAM", 0.001);
        public static readonly WeightUnit POUND = new WeightUnit("POUND", 0.453592);

        public double ConvertToBaseUnit(double value)
        {
            return value * toKgFactor;
        }

        public double ConvertFromBaseUnit(double baseValue)
        {
            return baseValue / toKgFactor;
        }

        public string GetUnitName()
        {
            return name;
        }
    }
}