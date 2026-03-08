namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Defines contract for all measurable units (Length, Weight, Future categories).
    /// </summary>
    public interface IMeasurable
    {
        /// <summary>
        /// Converts value to base unit of its category.
        /// </summary>
        double ConvertToBaseUnit(double value);

        /// <summary>
        /// Converts base unit value to this unit.
        /// </summary>
        double ConvertFromBaseUnit(double baseValue);

        /// <summary>
        /// Returns readable unit name.
        /// </summary>
        string GetUnitName();
    }
}