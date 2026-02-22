using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents measurement in Inches.
    /// </summary>
    public class Inches
    {
        private readonly double value;

        /// <summary>
        /// Initializes Inches object.
        /// </summary>
        public Inches(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Checks equality between two Inches objects.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Inches))
                return false;

            Inches other = (Inches)obj;

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