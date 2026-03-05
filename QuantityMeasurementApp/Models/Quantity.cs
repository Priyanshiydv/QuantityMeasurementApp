using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Wrapper class to maintain backward compatibility (UC1–UC9).
    /// Internally uses Generic Quantity<IMeasurable>.
    /// </summary>
    public class Quantity : QuantityGeneric<IMeasurable>
    {
        public Quantity(double value, IMeasurable unit)
            : base(value, unit)
        {
        }
        public static double Convert(double value, LengthUnit from, LengthUnit to)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            double baseValue = from.ConvertToBaseUnit(value);
            return to.ConvertFromBaseUnit(baseValue);
        }
    }
}