using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents measurement in Feet.
    /// </summary>
    public class Feet
    {
        private readonly double value;

        /// <summary>
        /// Initializes Feet object.
        /// </summary>
        public Feet(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Checks equality between two Feet objects.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Feet))
                return false;

            Feet other = (Feet)obj;

            return Math.Abs(this.value - other.value) < 0.0001;
        }

        /// <summary>
        /// Returns hash code.
        /// </summary>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}