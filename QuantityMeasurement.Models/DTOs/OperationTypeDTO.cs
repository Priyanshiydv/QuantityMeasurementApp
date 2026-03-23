namespace QuantityMeasurement.Models.DTOs
{
    /// <summary>
    /// Enum representing supported operation types.
    /// Used in QuantityInputDTO and QuantityResponseDTO.
    /// Provides type safety for operation types.
    /// UC17
    /// </summary>
    public enum OperationTypeDTO
    {
        /// <summary>
        /// Compare two quantities for equality.
        /// </summary>
        COMPARE,

        /// <summary>
        /// Convert quantity to target unit.
        /// </summary>
        CONVERT,

        /// <summary>
        /// Add two quantities together.
        /// </summary>
        ADD,

        /// <summary>
        /// Subtract second quantity from first.
        /// </summary>
        SUBTRACT,

        /// <summary>
        /// Divide first quantity by second.
        /// </summary>
        DIVIDE
    }
}