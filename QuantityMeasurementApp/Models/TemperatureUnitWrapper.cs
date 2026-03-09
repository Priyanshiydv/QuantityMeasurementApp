using System;

namespace QuantityMeasurementApp.Models
{
    public class TemperatureUnitWrapper : IMeasurable
    {
        public TemperatureUnit Unit { get; }

        public TemperatureUnitWrapper(TemperatureUnit unit)
        {
            Unit = unit;
        }

        public double ConvertToBaseUnit(double value)
        {
            return Unit.ConvertToBaseUnit(value);
        }

        public double ConvertFromBaseUnit(double baseValue)
        {
            return Unit.ConvertFromBaseUnit(baseValue);
        }

        public string GetUnitName()
        {
            return Unit.GetUnitName();
        }
    }
}