namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Provides conversion logic for WeightUnit.
    /// </summary>
    public static class WeightUnitExtensions
    {
        private const double KILOGRAM_FACTOR = 1.0;
        private const double GRAM_FACTOR = 0.001;       // 1 g = 0.001 kg
        private const double POUND_FACTOR = 0.453592;   // 1 lb = 0.453592 kg

        /// <summary>
        /// Returns conversion factor relative to kilogram.
        /// </summary>
        public static double GetConversionFactor(this WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.KILOGRAM => KILOGRAM_FACTOR,
                WeightUnit.GRAM => GRAM_FACTOR,
                WeightUnit.POUND => POUND_FACTOR,
                _ => throw new ArgumentException("Invalid Weight Unit")
            };
        }

        /// <summary>
        /// Converts given value to base unit (Kilogram).
        /// </summary>
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            switch (unit)
            {
                case WeightUnit.KILOGRAM:
                    return value;   // base = kilogram

                case WeightUnit.GRAM:
                    return value / 1000.0;

                case WeightUnit.POUND:
                    return value * 0.453592;   // VERY IMPORTANT

                default:
                    throw new ArgumentException("Invalid weight unit");
            }
        }

        /// <summary>
        /// Converts base unit value (Kilogram) to specified unit.
        /// </summary>
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            switch (unit)
            {
                case WeightUnit.KILOGRAM:
                    return baseValue;

                case WeightUnit.GRAM:
                    return baseValue * 1000.0;

                case WeightUnit.POUND:
                    return baseValue / 0.453592;

                default:
                    throw new ArgumentException("Invalid weight unit");
            }
        }
    }
}