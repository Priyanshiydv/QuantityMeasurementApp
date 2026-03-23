using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurement.Models.DTOs
{
    /// <summary>
    /// Input DTO for quantity measurement API operations.
    /// Sent by client as JSON request body.
    /// Contains two quantities for binary operations.
    /// UC17
    /// </summary>
    public class QuantityInputDTO
    {
        // ─── First Quantity ───────────────────────────────────

        /// <summary>
        /// First quantity value.
        /// e.g. 1.0
        /// </summary>
        [Required(ErrorMessage = "First value is required.")]
        public double FirstValue { get; set; }

        /// <summary>
        /// First quantity unit.
        /// e.g. "FEET", "KILOGRAM", "LITRE"
        /// </summary>
        [Required(ErrorMessage = "First unit is required.")]
        [StringLength(50, ErrorMessage =
            "First unit cannot exceed 50 characters.")]
        public string FirstUnit { get; set; } = string.Empty;

        /// <summary>
        /// First quantity measurement type.
        /// e.g. "Length", "Weight", "Volume", "Temperature"
        /// </summary>
        [Required(ErrorMessage =
            "First measurement type is required.")]
        [StringLength(50, ErrorMessage =
            "First measurement type cannot exceed 50 characters.")]
        public string FirstMeasurementType { get; set; }
            = string.Empty;

        // ─── Second Quantity ──────────────────────────────────

        /// <summary>
        /// Second quantity value.
        /// e.g. 12.0
        /// </summary>
        [Required(ErrorMessage = "Second value is required.")]
        public double SecondValue { get; set; }

        /// <summary>
        /// Second quantity unit.
        /// e.g. "INCHES", "GRAM", "MILLILITRE"
        /// </summary>
        [Required(ErrorMessage = "Second unit is required.")]
        [StringLength(50, ErrorMessage =
            "Second unit cannot exceed 50 characters.")]
        public string SecondUnit { get; set; } = string.Empty;

        /// <summary>
        /// Second quantity measurement type.
        /// e.g. "Length", "Weight", "Volume", "Temperature"
        /// </summary>
        [Required(ErrorMessage =
            "Second measurement type is required.")]
        [StringLength(50, ErrorMessage =
            "Second measurement type cannot exceed 50 characters.")]
        public string SecondMeasurementType { get; set; }
            = string.Empty;

        // ─── Optional Fields ──────────────────────────────────

        /// <summary>
        /// Optional target unit for conversion/addition.
        /// e.g. "INCHES", "KILOGRAM"
        /// If not provided first unit is used as target.
        /// </summary>
        public string? TargetUnit { get; set; }

        /// <summary>
        /// Operation type to perform.
        /// e.g. COMPARE, CONVERT, ADD, SUBTRACT, DIVIDE
        /// </summary>
        public OperationTypeDTO? OperationType { get; set; }
    }
}