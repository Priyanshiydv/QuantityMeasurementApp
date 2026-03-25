namespace QuantityMeasurement.Models.Models
{
    /// <summary>
    /// Internal model class used within the service layer.
    /// Represents a quantity with its value and unit.
    /// UC15
    /// </summary>
    public class QuantityModel
    {
        // ─── Properties ───────────────────────────────────────

        /// <summary>
        /// Numeric value of the quantity.
        /// e.g. 10, 2.5, 100
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Unit name as string.
        /// e.g. "FEET", "KILOGRAM", "LITRE", "CELSIUS"
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Measurement category.
        /// e.g. "Length", "Weight", "Volume", "Temperature"
        /// </summary>
        public string MeasurementType { get; set; }

        // ─── Constructors ──────────────────────────────────────

        /// <summary>
        /// Default constructor.
        /// </summary>
        public QuantityModel()
        {
            Unit = string.Empty;
            MeasurementType = string.Empty;
        }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        public QuantityModel(double value, string unit, string measurementType)
        {
            Value = value;
            Unit = unit;
            MeasurementType = measurementType;
        }

        // ─── ToString ─────────────────────────────────────────

        public override string ToString()
        {
            return $"{Value} {Unit} ({MeasurementType})";
        }
    }
}