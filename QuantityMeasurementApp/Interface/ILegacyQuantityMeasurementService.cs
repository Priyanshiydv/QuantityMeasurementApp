using QuantityMeasurementApp.Models;
using QuantityMeasurement.Service.Service;
using QuantityMeasurement.Service.Interfaces;

namespace QuantityMeasurementApp.Interface
{
    /// <summary>
    /// Service contract for generic quantity operations.
    /// UC10 - Generic Measurement Support
    /// </summary>
    public interface ILegacyQuantityMeasurementService
    {
        /// <summary>
        /// Checks equality between two quantities of same measurable type.
        /// </summary>
        bool AreEqual<U>(QuantityGeneric<U>? first, QuantityGeneric<U>? second)
            where U : IMeasurable;

        /// <summary>
        /// Adds two quantities and returns result in first operand's unit.
        /// </summary>
        QuantityGeneric<U> Add<U>(QuantityGeneric<U>? first, QuantityGeneric<U>? second)
            where U : IMeasurable;

        /// <summary>
        /// Adds two quantities and returns result in specified target unit.
        /// </summary>
        QuantityGeneric<U> Add<U>(QuantityGeneric<U>? first,
                           QuantityGeneric<U>? second,
                           U targetUnit)
            where U : IMeasurable;
    }
}