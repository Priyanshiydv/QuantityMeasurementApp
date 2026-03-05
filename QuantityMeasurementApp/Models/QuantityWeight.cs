using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Wrapper class for weight (UC9 backward compatibility)
    /// Internally uses QuantityGeneric<WeightUnit>
    /// </summary>
    public class QuantityWeight : QuantityGeneric<WeightUnit>
    {
        public QuantityWeight(double value, WeightUnit unit)
            : base(value, unit)
        {
        }

        public override bool Equals(object? obj)
        {
            if (obj is QuantityWeight other)
            {
                return base.Equals(other);
            }
            return false;
        }

        public  QuantityWeight Add(QuantityWeight? other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            var result = base.Add(other);
            return new QuantityWeight(result.Value, result.Unit);
        }

        public new QuantityWeight ConvertTo(WeightUnit targetUnit)
        {
            var result = base.ConvertTo(targetUnit);
            return new QuantityWeight(result.Value, result.Unit);
        }

        public QuantityWeight Add(QuantityWeight? other, WeightUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            var result = base.Add(other, targetUnit);
            return new QuantityWeight(result.Value, result.Unit);
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}