namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Enum representing supported length units.
    /// Enum is used for type safety and clarity.
    /// </summary>
    
    public enum LengthUnit //UC3
    {
        FEET ,
        INCHES ,
        YARDS , //UC4
        CENTIMETERS  //UC4  
    }
    /// <summary>
    /// Extension Methods for LengthUnit 
    /// Handles all unit conversion responsibility as per UC8
    /// </summary>
    public static class LengthUnitExtensions
    {
        /// <summary>
        /// Convert value from current unit to Base Unit (Feet)
        /// Example: 12 INCHES → 1 FEET
        /// </summary>
        public static double ToBase(this LengthUnit unit, double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Value must be finite");

            return unit switch
            {
                LengthUnit.FEET => value,
                LengthUnit.INCHES => value / 12,
                LengthUnit.YARDS => value * 3,
                LengthUnit.CENTIMETERS => value / 30.48,
                _ => throw new ArgumentException("Invalid Length Unit")
            };
        }

        /// <summary>
        /// Convert value from Base Unit (Feet) to Target Unit
        /// Example: 1 FEET → 12 INCHES
        /// </summary>
        public static double FromBase(this LengthUnit unit, double baseValue)
        {
            if (double.IsNaN(baseValue) || double.IsInfinity(baseValue))
                throw new ArgumentException("Base value must be finite");

            return unit switch
            {
                LengthUnit.FEET => baseValue,
                LengthUnit.INCHES => baseValue * 12,
                LengthUnit.YARDS => baseValue / 3,
                LengthUnit.CENTIMETERS => baseValue * 30.48,
                _ => throw new ArgumentException("Invalid Length Unit")
            };
        }
    }
}
