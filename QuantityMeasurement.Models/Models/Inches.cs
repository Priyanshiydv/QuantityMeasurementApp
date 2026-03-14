using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents measurement in Inches.
    /// Pure model class - no business logic.
    /// UC2
    /// </summary>
    public class Inches
    {
        // ─── Properties ───────────────────────────────────────

        /// <summary>
        /// Value of the inches measurement.
        /// </summary>
        public double Value { get; }

        // ─── Constructor ──────────────────────────────────────

        /// <summary>
        /// Initializes Inches object.
        /// </summary>
        public Inches(double value)
        {
            Value = value;
        }

        // ─── ToString ─────────────────────────────────────────

        public override string ToString()
        {
            return $"{Value} INCHES";
        }
        // ─── Equality ─────────────────────────────────────────

        /// <summary>
        /// Value based equality comparison.
        /// Two Inches are equal if values are within 0.0001.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null)    return false;
            if (obj is not Inches other) return false;

            return Math.Abs(Value - other.Value) < 0.0001;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}