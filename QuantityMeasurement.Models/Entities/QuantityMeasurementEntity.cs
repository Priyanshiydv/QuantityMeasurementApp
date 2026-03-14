using System;
using System.Text.Json;

namespace QuantityMeasurement.Models.Entities
{
    /// <summary>
    /// Entity class to store quantity measurement operation history.
    /// Holds operands, operation type, result and error info.
    /// UC15
    /// </summary>
    public class QuantityMeasurementEntity
    {
        // ─── Properties ───────────────────────────────────────

        /// <summary>
        /// Unique ID for each operation record.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// First operand value.
        /// e.g. 10 FEET
        /// </summary>
        public string? FirstOperand { get; set; }

        /// <summary>
        /// Second operand value (null for single operand ops).
        /// e.g. 5 INCHES
        /// </summary>
        public string? SecondOperand { get; set; }

        /// <summary>
        /// Operation type.
        /// e.g. "COMPARE", "CONVERT", "ADD", "SUBTRACT", "DIVIDE"
        /// </summary>
        public string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// Result of the operation as string.
        /// e.g. "15.00 FEET" or "true" or "3.0"
        /// </summary>
        public string? Result { get; set; }

        /// <summary>
        /// Whether an error occurred during the operation.
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// Error message if HasError is true.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Timestamp when operation was performed.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Type of measurement category.
        /// e.g. "Length", "Weight", "Volume", "Temperature"
        /// Added in UC16 for query filtering support.
        /// </summary>
        public string? MeasurementType { get; set; }

        

        // ─── Operation Type Constants ──────────────────────────

        public static class Operations
        {
            public const string COMPARE  = "COMPARE";
            public const string CONVERT  = "CONVERT";
            public const string ADD      = "ADD";
            public const string SUBTRACT = "SUBTRACT";
            public const string DIVIDE   = "DIVIDE";
        }

        // ─── Constructors ──────────────────────────────────────

        /// <summary>
        /// Default constructor (for serialization).
        /// </summary>
        public QuantityMeasurementEntity()
        {
            Id            = Guid.NewGuid().ToString();
            OperationType = string.Empty;
            Timestamp     = DateTime.Now;
            HasError      = false;
        }

        /// <summary>
        /// Constructor for single operand operations.
        /// e.g. CONVERT: 1 FEET -> INCHES
        /// </summary>
        public QuantityMeasurementEntity(
            string firstOperand,
            string operationType,
            string result,
            string? measurementType = null)
        {
            Id            = Guid.NewGuid().ToString();
            FirstOperand  = firstOperand;
            OperationType = operationType;
            Result        = result;
            HasError      = false;
            Timestamp     = DateTime.Now;
            MeasurementType = measurementType;
        }

        /// <summary>
        /// Constructor for binary operand operations.
        /// e.g. ADD: 1 FEET + 12 INCHES
        /// </summary>
        public QuantityMeasurementEntity(
            string firstOperand,
            string secondOperand,
            string operationType,
            string result,
            string? measurementType = null)
        {
            Id             = Guid.NewGuid().ToString();
            FirstOperand   = firstOperand;
            SecondOperand  = secondOperand;
            OperationType  = operationType;
            Result         = result;
            HasError       = false;
            Timestamp      = DateTime.Now;
            MeasurementType = measurementType;
        }

        /// <summary>
        /// Constructor for error scenarios.
        /// </summary>
        public QuantityMeasurementEntity(
            string operationType,
            string errorMessage,
            bool hasError,
            string? measurementType = null)
        {
            Id            = Guid.NewGuid().ToString();
            OperationType = operationType;
            ErrorMessage  = errorMessage;
            HasError      = hasError;
            Timestamp     = DateTime.Now;
            MeasurementType = measurementType;
        }

        // ─── ToString ─────────────────────────────────────────

        public override string ToString()
        {
            if (HasError)
                return $"[{Timestamp:HH:mm:ss}] {OperationType} => ERROR: {ErrorMessage}";

            if (SecondOperand != null)
                return $"[{Timestamp:HH:mm:ss}] {OperationType}: {FirstOperand} & {SecondOperand} => {Result}";

            return $"[{Timestamp:HH:mm:ss}] {OperationType}: {FirstOperand} => {Result}";
        }
    }
}