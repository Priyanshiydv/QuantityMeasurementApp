using System;

namespace QuantityMeasurement.Models.Exceptions
{
    /// <summary>
    /// Custom exception for database related errors.
    /// Thrown when database operations fail during execution.
    /// Extends QuantityMeasurementException for consistent
    /// exception hierarchy across the application.
    /// UC16
    /// </summary>
    public class DatabaseException : QuantityMeasurementException
    {
        // ─── Error Code Constants ──────────────────────────────

        /// <summary>
        /// Specific error codes for database operations.
        /// </summary>
        public static class DatabaseErrorCodes
        {
            // Connection errors
            public const string CONNECTION_FAILED    = "CONNECTION_FAILED";
            public const string CONNECTION_TIMEOUT   = "CONNECTION_TIMEOUT";
            public const string POOL_EXHAUSTED       = "POOL_EXHAUSTED";

            // Query errors
            public const string QUERY_FAILED         = "QUERY_FAILED";
            public const string INSERT_FAILED        = "INSERT_FAILED";
            public const string SELECT_FAILED        = "SELECT_FAILED";
            public const string DELETE_FAILED        = "DELETE_FAILED";
            public const string UPDATE_FAILED        = "UPDATE_FAILED";

            // Transaction errors
            public const string TRANSACTION_FAILED   = "TRANSACTION_FAILED";
            public const string ROLLBACK_FAILED      = "ROLLBACK_FAILED";

            // Schema errors
            public const string SCHEMA_NOT_FOUND     = "SCHEMA_NOT_FOUND";
            public const string TABLE_NOT_FOUND      = "TABLE_NOT_FOUND";

            // General errors
            public const string GENERAL_DB_ERROR     = "GENERAL_DB_ERROR";
        }

        // ─── Properties ───────────────────────────────────────

        /// <summary>
        /// SQL query that caused the exception (if available).
        /// Useful for debugging database issues.
        /// </summary>
        public string? FailedQuery { get; }

        /// <summary>
        /// Database name where error occurred.
        /// </summary>
        public string? DatabaseName { get; }

        // ─── Constructors ──────────────────────────────────────

        /// <summary>
        /// Constructor with message only.
        /// </summary>
        public DatabaseException(string message)
            : base(message, DatabaseErrorCodes.GENERAL_DB_ERROR)
        {
        }

        /// <summary>
        /// Constructor with message and error code.
        /// </summary>
        public DatabaseException(string message, string errorCode)
            : base(message, errorCode)
        {
        }

        /// <summary>
        /// Constructor with message and inner exception.
        /// Used for wrapping ADO.NET SqlException.
        /// </summary>
        public DatabaseException(
            string message,
            Exception innerException)
            : base(
                message,
                DatabaseErrorCodes.GENERAL_DB_ERROR,
                innerException)
        {
        }

        /// <summary>
        /// Constructor with message, error code and inner exception.
        /// Most detailed constructor for database errors.
        /// </summary>
        public DatabaseException(
            string message,
            string errorCode,
            Exception innerException)
            : base(message, errorCode, innerException)
        {
        }

        /// <summary>
        /// Constructor with message, error code,
        /// failed query and database name.
        /// Provides maximum context for debugging.
        /// </summary>
        public DatabaseException(
            string message,
            string errorCode,
            string? failedQuery,
            string? databaseName)
            : base(message, errorCode)
        {
            FailedQuery  = failedQuery;
            DatabaseName = databaseName;
        }

        /// <summary>
        /// Full constructor with all details including
        /// inner exception, failed query and database name.
        /// </summary>
        public DatabaseException(
            string message,
            string errorCode,
            Exception innerException,
            string? failedQuery,
            string? databaseName)
            : base(message, errorCode, innerException)
        {
            FailedQuery  = failedQuery;
            DatabaseName = databaseName;
        }

        // ─── ToString ─────────────────────────────────────────

        public override string ToString()
        {
            string baseMessage =
                $"DatabaseException [{ErrorCode}]: {Message}";

            if (DatabaseName != null)
                baseMessage += $" | Database: {DatabaseName}";

            if (FailedQuery != null)
                baseMessage += $" | Query: {FailedQuery}";

            return baseMessage;
        }
    }
}