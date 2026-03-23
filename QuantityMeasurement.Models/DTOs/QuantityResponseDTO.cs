using QuantityMeasurement.Models.Entities;

namespace QuantityMeasurement.Models.DTOs
{
    /// <summary>
    /// Response DTO for quantity measurement API operations.
    /// Returned by API after each operation as JSON.
    /// Contains input values, operation type and result.
    /// UC17
    /// </summary>
    public class QuantityResponseDTO
    {
        // ─── Input Values ─────────────────────────────────────

        /// <summary>
        /// First operand value.
        /// e.g. 1.0
        /// </summary>
        public double FirstValue { get; set; }

        /// <summary>
        /// First operand unit.
        /// e.g. "FEET"
        /// </summary>
        public string FirstUnit { get; set; } = string.Empty;

        /// <summary>
        /// First operand measurement type.
        /// e.g. "Length"
        /// </summary>
        public string FirstMeasurementType { get; set; }
            = string.Empty;

        /// <summary>
        /// Second operand value.
        /// e.g. 12.0
        /// </summary>
        public double SecondValue { get; set; }

        /// <summary>
        /// Second operand unit.
        /// e.g. "INCHES"
        /// </summary>
        public string SecondUnit { get; set; } = string.Empty;

        /// <summary>
        /// Second operand measurement type.
        /// e.g. "Length"
        /// </summary>
        public string SecondMeasurementType { get; set; }
            = string.Empty;

        // ─── Operation Info ───────────────────────────────────

        /// <summary>
        /// Type of operation performed.
        /// e.g. "COMPARE", "CONVERT", "ADD"
        /// </summary>
        public string Operation { get; set; } = string.Empty;

        // ─── Result ───────────────────────────────────────────

        /// <summary>
        /// String result for compare operations.
        /// e.g. "True" or "False"
        /// </summary>
        public string? ResultString { get; set; }

        /// <summary>
        /// Numeric result for arithmetic operations.
        /// e.g. 2.0 for addition result
        /// </summary>
        public double ResultValue { get; set; }

        /// <summary>
        /// Result unit for arithmetic operations.
        /// e.g. "FEET" for addition result
        /// </summary>
        public string? ResultUnit { get; set; }

        /// <summary>
        /// Result measurement type.
        /// e.g. "Length"
        /// </summary>
        public string? ResultMeasurementType { get; set; }

        // ─── Error Info ───────────────────────────────────────

        /// <summary>
        /// Error message if operation failed.
        /// null if no error occurred.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Whether operation resulted in error.
        /// true if error occurred, false otherwise.
        /// </summary>
        public bool HasError { get; set; }

        // ─── Factory Methods ──────────────────────────────────

        /// <summary>
        /// Creates QuantityResponseDTO from entity.
        /// Maps entity fields to response DTO fields.
        /// Used in history endpoints.
        /// UC17
        /// </summary>
        public static QuantityResponseDTO FromEntity(
            QuantityMeasurementEntity entity)
        {
            return new QuantityResponseDTO
            {
                Operation    = entity.OperationType,
                ResultString = entity.Result,
                ErrorMessage = entity.ErrorMessage,
                HasError     = entity.HasError
            };
        }

        /// <summary>
        /// Creates list of DTOs from entity list.
        /// Uses LINQ for efficient mapping.
        /// UC17
        /// </summary>
        public static List<QuantityResponseDTO> FromEntityList(
            List<QuantityMeasurementEntity> entities)
        {
            return entities
                .Select(e => new QuantityResponseDTO
                {
                    Operation    = e.OperationType,
                    ResultString = e.Result,
                    ErrorMessage = e.ErrorMessage,
                    HasError     = e.HasError,
                    FirstMeasurementType =
                        e.MeasurementType ?? string.Empty
                })
                .ToList();
        }
    }
}