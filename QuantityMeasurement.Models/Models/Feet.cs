using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents measurement in Feet.
    /// Pure model class - no business logic.
    /// UC1
    /// </summary>
    public class Feet
    {
        // ─── Properties ───────────────────────────────────────

        /// <summary>
        /// Value of the feet measurement.
        /// </summary>
        public double Value { get; }

        // ─── Constructor ──────────────────────────────────────

        /// <summary>
        /// Initializes Feet object.
        /// </summary>
        public Feet(double value)
        {
            Value = value;
        }

        // ─── ToString ─────────────────────────────────────────

        public override string ToString()
        {
            return $"{Value} FEET";
        }

         // ─── Equality ─────────────────────────────────────────

        /// <summary>
        /// Value based equality comparison.
        /// Two Feet are equal if values are within 0.0001.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null)    return false;
            if (obj is not Feet other) return false;

            return Math.Abs(Value - other.Value) < 0.0001;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}