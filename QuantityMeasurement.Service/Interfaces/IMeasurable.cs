namespace QuantityMeasurement.Service.Interfaces
{
    /// <summary>
    /// Defines contract for all measurable units.
    /// Length, Weight, Volume, Temperature categories.
    /// UC10, UC16
    /// </summary>
    public interface IMeasurable
    {
        // ─── Core Methods ─────────────────────────────────────

        /// <summary>
        /// Converts value to base unit of its category.
        /// e.g. INCHES → FEET, GRAM → KILOGRAM
        /// </summary>
        double ConvertToBaseUnit(double value);

        /// <summary>
        /// Converts base unit value back to this unit.
        /// e.g. FEET → INCHES, KILOGRAM → GRAM
        /// </summary>
        double ConvertFromBaseUnit(double baseValue);

        /// <summary>
        /// Returns readable unit name.
        /// e.g. "FEET", "KILOGRAM", "LITRE"
        /// </summary>
        string GetUnitName();

        // ─── Arithmetic Support ───────────────────────────────

        /// <summary>
        /// Delegate indicating arithmetic is supported.
        /// Default: true (length, weight, volume)
        /// Temperature overrides to false.
        /// </summary>
        ArithmeticSupportDelegate GetArithmeticSupport()
            => () => true;

        /// <summary>
        /// Validates arithmetic operation is supported.
        /// Default: allow all operations.
        /// Temperature overrides to throw exception.
        /// </summary>
        void ValidateOperationSupport(string operation)
        {
            // Default: all operations supported
            // Temperature will override this
        }
    }
}