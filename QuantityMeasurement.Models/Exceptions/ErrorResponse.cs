namespace QuantityMeasurement.Models.Exceptions
{
    /// <summary>
    /// Standardized error response model.
    /// Returned for all API exceptions.
    /// Used by GlobalExceptionMiddleware.
    /// UC17
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Timestamp when error occurred.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// HTTP status code.
        /// e.g. 400, 500
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// HTTP status description.
        /// e.g. "BadRequest", "InternalServerError"
        /// </summary>
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Error message from exception.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Request path that caused error.
        /// e.g. "/api/v1/quantities/compare"
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Application specific error code.
        /// e.g. "INVALID_UNIT", "CROSS_CATEGORY"
        /// </summary>
        public string ErrorCode { get; set; } = string.Empty;
    }
}