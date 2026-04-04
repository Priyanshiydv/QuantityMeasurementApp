using QuantityMeasurement.QMAService.Models;

namespace QuantityMeasurement.QMAService.Interfaces
{
    public interface IQMAService
    {
        QuantityResponse Compare(
            QuantityInput input, int? userId = null);
        QuantityResponse Convert(
            QuantityInput input, int? userId = null);
        QuantityResponse Add(
            QuantityInput input, int? userId = null);
        QuantityResponse Subtract(
            QuantityInput input, int? userId = null);
        QuantityResponse Divide(
            QuantityInput input, int? userId = null);
    }
}