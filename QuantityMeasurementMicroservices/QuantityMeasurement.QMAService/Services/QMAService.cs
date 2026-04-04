using QuantityMeasurement.QMAService.Data;
using QuantityMeasurement.QMAService.Models;
using QuantityMeasurement.QMAService.Interfaces;

namespace QuantityMeasurement.QMAService.Services
{
    public class QMAService : IQMAService
    {
        private readonly QMADbContext _db;
        private readonly ILogger<QMAService> _logger;
        private readonly ICacheService _cache;


        public QMAService(
            QMADbContext db,
            ILogger<QMAService> logger,
            ICacheService cache)
        {
            _db     = db;
            _logger = logger;
            _cache  = cache;
        }

        // ── Compare ───────────────────────────────────
        public QuantityResponse Compare(
            QuantityInput input, int? userId = null)
        {
            // ── Check Cache ───────────────────────────────
            string cacheKey = $"compare_{input.FirstValue}" +
                $"_{input.FirstUnit}_{input.SecondValue}" +
                $"_{input.SecondUnit}";

            var cached = _cache.GetAsync(cacheKey).Result;
            if (cached != null)
            {
                _logger.LogInformation(
                    "[QMAService] Cache HIT: {Key}", cacheKey);
                return System.Text.Json.JsonSerializer
                    .Deserialize<QuantityResponse>(cached)!;
            }
            double base1 = ConvertToBase(
                input.FirstValue,
                input.FirstUnit,
                input.FirstMeasurementType);

            double base2 = ConvertToBase(
                input.SecondValue,
                input.SecondUnit,
                input.SecondMeasurementType);

            bool isEqual = Math.Abs(base1 - base2) < 0.0001;

            SaveEntity(
                $"{input.FirstValue} {input.FirstUnit}" +
                $" ({input.FirstMeasurementType})",
                $"{input.SecondValue} {input.SecondUnit}" +
                $" ({input.SecondMeasurementType})",
                "COMPARE",
                isEqual.ToString(),
                input.FirstMeasurementType,
                userId);

            var result = new QuantityResponse
            {
                FirstValue           = input.FirstValue,
                FirstUnit            = input.FirstUnit,
                FirstMeasurementType = input.FirstMeasurementType,
                SecondValue          = input.SecondValue,
                SecondUnit           = input.SecondUnit,
                Operation            = "COMPARE",
                ResultString         = isEqual.ToString(),
                HasError             = false
            };
            // ── Save to Cache ─────────────────────────────
            _cache.SetAsync(cacheKey,
                System.Text.Json.JsonSerializer.Serialize(result),
                TimeSpan.FromMinutes(10)).Wait();

            return result;
        }

        // ── Convert ───────────────────────────────────
        public QuantityResponse Convert(
            QuantityInput input, int? userId = null)
        {
            string cacheKey = $"convert_{input.FirstValue}" +
                $"_{input.FirstUnit}_{input.TargetUnit}";
            
            string targetUnit = input.TargetUnit
                ?? input.SecondUnit;

            double baseValue = ConvertToBase(
                input.FirstValue,
                input.FirstUnit,
                input.FirstMeasurementType);

            double converted = ConvertFromBase(
                baseValue,
                targetUnit,
                input.FirstMeasurementType);

            string result =
                $"{converted:F2} {targetUnit}";

            SaveEntity(
                $"{input.FirstValue} {input.FirstUnit}" +
                $" ({input.FirstMeasurementType})",
                null,
                "CONVERT",
                result,
                input.FirstMeasurementType,
                userId);

            return new QuantityResponse
            {
                FirstValue           = input.FirstValue,
                FirstUnit            = input.FirstUnit,
                FirstMeasurementType = input.FirstMeasurementType,
                Operation            = "CONVERT",
                ResultString         = result,
                ResultValue          = converted,
                ResultUnit           = targetUnit,
                HasError             = false
            };
            
            
        }

        // ── Add ───────────────────────────────────────
        public QuantityResponse Add(
            QuantityInput input, int? userId = null)
        {
            string targetUnit = input.TargetUnit
                ?? input.FirstUnit;

            double base1 = ConvertToBase(
                input.FirstValue,
                input.FirstUnit,
                input.FirstMeasurementType);

            double base2 = ConvertToBase(
                input.SecondValue,
                input.SecondUnit,
                input.SecondMeasurementType);

            double sum = ConvertFromBase(
                base1 + base2,
                targetUnit,
                input.FirstMeasurementType);

            string result = $"{sum:F2} {targetUnit}";

            SaveEntity(
                $"{input.FirstValue} {input.FirstUnit}" +
                $" ({input.FirstMeasurementType})",
                $"{input.SecondValue} {input.SecondUnit}" +
                $" ({input.SecondMeasurementType})",
                "ADD",
                result,
                input.FirstMeasurementType,
                userId);

            return new QuantityResponse
            {
                FirstValue           = input.FirstValue,
                FirstUnit            = input.FirstUnit,
                FirstMeasurementType = input.FirstMeasurementType,
                SecondValue          = input.SecondValue,
                SecondUnit           = input.SecondUnit,
                Operation            = "ADD",
                ResultString         = result,
                ResultValue          = sum,
                ResultUnit           = targetUnit,
                HasError             = false
            };
        }

        // ── Subtract ──────────────────────────────────
        public QuantityResponse Subtract(
            QuantityInput input, int? userId = null)
        {
            double base1 = ConvertToBase(
                input.FirstValue,
                input.FirstUnit,
                input.FirstMeasurementType);

            double base2 = ConvertToBase(
                input.SecondValue,
                input.SecondUnit,
                input.SecondMeasurementType);

            double diff = ConvertFromBase(
                base1 - base2,
                input.FirstUnit,
                input.FirstMeasurementType);

            string result =
                $"{diff:F2} {input.FirstUnit}";

            SaveEntity(
                $"{input.FirstValue} {input.FirstUnit}" +
                $" ({input.FirstMeasurementType})",
                $"{input.SecondValue} {input.SecondUnit}" +
                $" ({input.SecondMeasurementType})",
                "SUBTRACT",
                result,
                input.FirstMeasurementType,
                userId);

            return new QuantityResponse
            {
                FirstValue           = input.FirstValue,
                FirstUnit            = input.FirstUnit,
                FirstMeasurementType = input.FirstMeasurementType,
                SecondValue          = input.SecondValue,
                SecondUnit           = input.SecondUnit,
                Operation            = "SUBTRACT",
                ResultString         = result,
                ResultValue          = diff,
                ResultUnit           = input.FirstUnit,
                HasError             = false
            };
        }

        // ── Divide ────────────────────────────────────
        public QuantityResponse Divide(
            QuantityInput input, int? userId = null)
        {
            double base1 = ConvertToBase(
                input.FirstValue,
                input.FirstUnit,
                input.FirstMeasurementType);

            double base2 = ConvertToBase(
                input.SecondValue,
                input.SecondUnit,
                input.SecondMeasurementType);

            if (Math.Abs(base2) < 0.0001)
                throw new InvalidOperationException(
                    "Division by zero.");

            double result = base1 / base2;

            SaveEntity(
                $"{input.FirstValue} {input.FirstUnit}" +
                $" ({input.FirstMeasurementType})",
                $"{input.SecondValue} {input.SecondUnit}" +
                $" ({input.SecondMeasurementType})",
                "DIVIDE",
                $"{result:F2}",
                input.FirstMeasurementType,
                userId);

            return new QuantityResponse
            {
                FirstValue           = input.FirstValue,
                FirstUnit            = input.FirstUnit,
                FirstMeasurementType = input.FirstMeasurementType,
                SecondValue          = input.SecondValue,
                SecondUnit           = input.SecondUnit,
                Operation            = "DIVIDE",
                ResultString         = $"{result:F2}",
                ResultValue          = result,
                ResultUnit           = "SCALAR",
                HasError             = false
            };
        }

        // ── Save Entity ───────────────────────────────
        private void SaveEntity(
            string? firstOperand,
            string? secondOperand,
            string operationType,
            string? result,
            string? measurementType,
            int? userId)
        {
            var entity = new MeasurementEntity
            {
                FirstOperand    = firstOperand,
                SecondOperand   = secondOperand,
                OperationType   = operationType,
                Result          = result,
                MeasurementType = measurementType,
                UserId          = userId,
                Timestamp       = DateTime.UtcNow
            };

            _db.Measurements.Add(entity);
            _db.SaveChanges();

            _logger.LogInformation(
                "[QMAService] Saved: {Op}", operationType);
        }

        // ── Conversion Helpers ────────────────────────
        private double ConvertToBase(
            double value, string unit, string type)
        {
            return type.ToUpper() switch
            {
                "LENGTH" => unit.ToUpper() switch
                {
                    "FEET"        => value * 1.0,
                    "INCHES"      => value / 12.0,
                    "YARDS"       => value * 3.0,
                    "CENTIMETERS" => value / 30.48,
                    _ => throw new ArgumentException(
                        $"Unknown unit: {unit}")
                },
                "WEIGHT" => unit.ToUpper() switch
                {
                    "KILOGRAM" => value,
                    "GRAM"     => value / 1000.0,
                    "POUND"    => value * 0.453592,
                    _ => throw new ArgumentException(
                        $"Unknown unit: {unit}")
                },
                "VOLUME" => unit.ToUpper() switch
                {
                    "LITRE"      => value,
                    "MILLILITRE" => value / 1000.0,
                    "GALLON"     => value * 3.78541,
                    _ => throw new ArgumentException(
                        $"Unknown unit: {unit}")
                },
                "TEMPERATURE" => unit.ToUpper() switch
                {
                    "CELSIUS"    => value,
                    "FAHRENHEIT" => (value - 32) * 5.0 / 9,
                    "KELVIN"     => value - 273.15,
                    _ => throw new ArgumentException(
                        $"Unknown unit: {unit}")
                },
                _ => throw new ArgumentException(
                    $"Unknown type: {type}")
            };
        }

        private double ConvertFromBase(
            double value, string unit, string type)
        {
            return type.ToUpper() switch
            {
                "LENGTH" => unit.ToUpper() switch
                {
                    "FEET"        => value,
                    "INCHES"      => value * 12.0,
                    "YARDS"       => value / 3.0,
                    "CENTIMETERS" => value * 30.48,
                    _ => throw new ArgumentException(
                        $"Unknown unit: {unit}")
                },
                "WEIGHT" => unit.ToUpper() switch
                {
                    "KILOGRAM" => value,
                    "GRAM"     => value * 1000.0,
                    "POUND"    => value / 0.453592,
                    _ => throw new ArgumentException(
                        $"Unknown unit: {unit}")
                },
                "VOLUME" => unit.ToUpper() switch
                {
                    "LITRE"      => value,
                    "MILLILITRE" => value * 1000.0,
                    "GALLON"     => value / 3.78541,
                    _ => throw new ArgumentException(
                        $"Unknown unit: {unit}")
                },
                "TEMPERATURE" => unit.ToUpper() switch
                {
                    "CELSIUS"    => value,
                    "FAHRENHEIT" => value * 9.0 / 5 + 32,
                    "KELVIN"     => value + 273.15,
                    _ => throw new ArgumentException(
                        $"Unknown unit: {unit}")
                },
                _ => throw new ArgumentException(
                    $"Unknown type: {type}")
            };
        }
    }
}