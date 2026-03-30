using System;

namespace QuantityMeasurement.Models.Exceptions
{
    /// <summary>
    /// Custom exception for quantity measurement operations.
    /// Thrown when invalid measurements, unit conversions,
    /// or other quantity-related operations fail.
    /// UC15
    /// </summary>
    public class QuantityMeasurementException : Exception
    {
        // ─── Error Code Constants ──────────────────────────────

        public static class ErrorCodes
        {
            public const string INVALID_VALUE          = "INVALID_VALUE";
            public const string INVALID_UNIT           = "INVALID_UNIT";
            public const string INVALID_CATEGORY       = "INVALID_CATEGORY";
            public const string CROSS_CATEGORY         = "CROSS_CATEGORY";
            public const string UNSUPPORTED_OPERATION  = "UNSUPPORTED_OPERATION";
            public const string DIVISION_BY_ZERO       = "DIVISION_BY_ZERO";
            public const string NULL_QUANTITY          = "NULL_QUANTITY";
        }

        // ─── Properties ───────────────────────────────────────

        /// <summary>
        /// Error code to identify type of error.
        /// </summary>
        public string ErrorCode { get; }

        // ─── Constructors ──────────────────────────────────────

        /// <summary>
        /// Constructor with message only.
        /// </summary>
        public QuantityMeasurementException(string message)
            : base(message)
        {
            ErrorCode = "GENERAL_ERROR";
        }

        /// <summary>
        /// Constructor with message and error code.
        /// </summary>
        public QuantityMeasurementException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Constructor with message and inner exception.
        /// Used for wrapping existing exceptions.
        /// </summary>
        public QuantityMeasurementException(string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = "GENERAL_ERROR";
        }

        /// <summary>
        /// Constructor with message, error code and inner exception.
        /// </summary>
        public QuantityMeasurementException(string message, string errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        // ─── ToString ─────────────────────────────────────────

        public override string ToString()
        {
            return $"QuantityMeasurementException [{ErrorCode}]: {Message}";
        }
    }
}