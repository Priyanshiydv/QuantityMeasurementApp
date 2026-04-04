using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurement.QMAService.Models
{
    public class QuantityInput
    {
        [Required]
        public double FirstValue { get; set; }

        [Required]
        public string FirstUnit { get; set; }
            = string.Empty;

        [Required]
        public string FirstMeasurementType { get; set; }
            = string.Empty;

        public double SecondValue { get; set; }

        public string SecondUnit { get; set; }
            = string.Empty;

        public string SecondMeasurementType { get; set; }
            = string.Empty;

        public string? TargetUnit { get; set; }
    }

    public class QuantityResponse
    {
        public double FirstValue  { get; set; }
        public string FirstUnit   { get; set; } = string.Empty;
        public string FirstMeasurementType { get; set; }
            = string.Empty;
        public double SecondValue { get; set; }
        public string SecondUnit  { get; set; } = string.Empty;
        public string SecondMeasurementType { get; set; }
            = string.Empty;
        public string Operation   { get; set; } = string.Empty;
        public string ResultString { get; set; } = string.Empty;
        public double ResultValue { get; set; }
        public string ResultUnit  { get; set; } = string.Empty;
        public bool   HasError    { get; set; } = false;
        public string? ErrorMessage { get; set; }
    }

    public class HistoryResponse
    {
        public string Operation   { get; set; } = string.Empty;
        public string? FirstUnit  { get; set; }
        public string? SecondUnit { get; set; }
        public string? ResultString { get; set; }
        public string? FirstMeasurementType { get; set; }
        public bool    HasError   { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}