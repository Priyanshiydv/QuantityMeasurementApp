using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Interface;
using QuantityMeasurement.Service.Service;
using QuantityMeasurement.Service.Interfaces;

namespace QuantityMeasurementApp.Services
{
    /// <summary>
    /// Service implementation for generic quantity operations.
    /// Handles Length, Weight, and any future measurable category.
    /// </summary>
    public class QuantityMeasurementService : ILegacyQuantityMeasurementService
    {
        /// <summary>
        /// Generic equality comparison.
        /// Returns false if any quantity is null.
        /// </summary>
        public bool AreEqual<U>(QuantityGeneric<U>? first, QuantityGeneric<U>? second)
            where U : IMeasurable
        {
            if (first == null || second == null)
                return false;

            return first.Equals(second);
        }

        /// <summary>
        /// Adds two quantities and returns result
        /// in the first quantity's unit.
        /// </summary>
        public QuantityGeneric<U> Add<U>(QuantityGeneric<U>? first,
                                  QuantityGeneric<U>? second)
            where U : IMeasurable
        {
            if (first == null || second == null)
                throw new ArgumentNullException("Quantity cannot be null.");

            return first.Add(second);
        }

        /// <summary>
        /// Adds two quantities and returns result
        /// in the specified target unit.
        /// </summary>
        public QuantityGeneric<U> Add<U>(QuantityGeneric<U>? first,QuantityGeneric<U>? second, U targetUnit)
            where U : IMeasurable
        {
            if (first == null || second == null)
                throw new ArgumentNullException("Quantity cannot be null.");
             if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null.");

            return first.Add(second, targetUnit);
        }




//===========================UC1 AND UC2===========================
        public bool AreEqual(Feet first, Feet second)
        {
            if (first == null || second == null)
                return false;

            return first.Equals(second);
        }

        public bool AreEqual(Inches first, Inches second)
        {
            if (first == null || second == null)
                return false;

            return first.Equals(second);
        }

        public bool AreEqual(Quantity first, Quantity second)
        {
            if (first == null || second == null)
                return false;

            return first.Equals(second);
        }

        public Quantity Add(Quantity? first, Quantity? second)
        {
            if (first == null || second == null)
                throw new ArgumentNullException();

            var result = first.Add(second); // returns QuantityGeneric<IMeasurable>

            return new Quantity(result.Value, result.Unit);
        }

        public Quantity Add(Quantity? first, Quantity? second, LengthUnit targetUnit)
        {
            if (first == null || second == null)
                throw new ArgumentNullException();

            var result = first.Add(second, targetUnit);

            return new Quantity(result.Value, result.Unit);
        }
        

    }
}