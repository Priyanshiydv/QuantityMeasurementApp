using System;
namespace QuantityMeasurementApp.Models
{
    public class TemperatureQuantity
    {
        public double Value { get; }
        public TemperatureUnit Unit { get; }

        public TemperatureQuantity(double value, TemperatureUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public TemperatureQuantity ConvertTo(TemperatureUnit target)
        {
            double converted = Unit switch
            {
                TemperatureUnit.CELSIUS => target switch
                {
                    TemperatureUnit.CELSIUS => Value,
                    TemperatureUnit.FAHRENHEIT => Value * 9 / 5 + 32,
                    TemperatureUnit.KELVIN => Value + 273.15,
                    _ => throw new ArgumentException("Invalid Temperature Unit")
                },
                TemperatureUnit.FAHRENHEIT => target switch
                {
                    TemperatureUnit.CELSIUS => (Value - 32) * 5 / 9,
                    TemperatureUnit.FAHRENHEIT => Value,
                    TemperatureUnit.KELVIN => (Value - 32) * 5 / 9 + 273.15,
                    _ => throw new ArgumentException("Invalid Temperature Unit")
                },
                TemperatureUnit.KELVIN => target switch
                {
                    TemperatureUnit.CELSIUS => Value - 273.15,
                    TemperatureUnit.FAHRENHEIT => (Value - 273.15) * 9 / 5 + 32,
                    TemperatureUnit.KELVIN => Value,
                    _ => throw new ArgumentException("Invalid Temperature Unit")
                },
                _ => throw new ArgumentException("Invalid Temperature Unit")
            };

            return new TemperatureQuantity(converted, target);
        }

        public override string ToString() => $"{Value} {Unit}";
    }
}