namespace QuantityMeasurementApp.Models
{
    // represents feet measurement value
    public class Feet
    {
        // private readonly value 
        private readonly double value;

        // constructor to set feet value
        public Feet(double value)
        {
            this.value = value;
        }

        // method to get value
        public double GetValue()
        {
            return value;
        }

        // override Equals for value comparison (UC1)
        public override bool Equals(object? obj)
        {
            // same reference check 
            if (this == obj)
                return true;

            // null and type check (type safety)
            if (obj == null || obj.GetType() != typeof(Feet))
                return false;

            // safe casting
            Feet other = (Feet)obj;

            // compare double values properly
            return this.value == other.value;
        }

        // override GetHashCode 
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}