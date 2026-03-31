using System;
using QuantityMeasurement.Models.Models;
using QuantityMeasurement.Models.UnitDTOs;
using QuantityMeasurement.Models.Entities;
using QuantityMeasurement.Models.Exceptions;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Service.Interfaces;

namespace QuantityMeasurement.Service.Service
{
    /// <summary>
    /// Service implementation for quantity measurement operations.
    /// Accepts QuantityDTO → converts to QuantityModel internally
    /// → performs operations → returns QuantityDTO.
    /// UC15
    /// </summary>
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        // ─── Repository Dependency ────────────────────────────

        private readonly IQuantityMeasurementRepository _repository;

        // ─── Constructor (Dependency Injection) ───────────────

        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        // ─── Compare ──────────────────────────────────────────

        public QuantityDTO Compare(QuantityDTO first, QuantityDTO second, int? userId = null)
        {
            try
            {
                // Step 1: Validate DTOs
                ValidateDTO(first,  "First quantity");
                ValidateDTO(second, "Second quantity");

                // Step 2: Convert DTO -> QuantityModel
                QuantityModel model1 = ToModel(first);
                QuantityModel model2 = ToModel(second);

                // Step 3: Validate same category
                ValidateSameCategory(model1, model2);

                // Step 4: Compare using models
                double base1   = ConvertToBase(model1);
                double base2   = ConvertToBase(model2);
                bool   isEqual = Math.Abs(base1 - base2) < 0.0001;

                // Step 5: Save to repository
                var entity = new QuantityMeasurementEntity(
                    model1.ToString(),
                    model2.ToString(),
                    QuantityMeasurementEntity.Operations.COMPARE,
                    isEqual.ToString(),
                    model1.MeasurementType
                )
                { UserId = userId };
                _repository.Save(entity);

                // Step 6: Return result as QuantityDTO
                return new QuantityDTO(
                    isEqual ? 1 : 0,
                    isEqual.ToString(),
                    "Result"
                );
            }
            catch (QuantityMeasurementException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException(
                    ex.Message,
                    QuantityMeasurementException.ErrorCodes.INVALID_VALUE,
                    ex
                );
            }
        }

        // ─── Convert ──────────────────────────────────────────

        public QuantityDTO Convert(QuantityDTO quantity, string targetUnit, int? userId = null)
        {
            try
            {
                // Step 1: Validate DTO
                ValidateDTO(quantity, "Quantity");

                if (string.IsNullOrWhiteSpace(targetUnit))
                    throw new QuantityMeasurementException(
                        "Target unit cannot be empty.",
                        QuantityMeasurementException.ErrorCodes.INVALID_UNIT
                    );

                // Step 2: Convert DTO -> QuantityModel
                QuantityModel model = ToModel(quantity);

                // Step 3: Convert to base then to target
                double baseValue      = ConvertToBase(model);
                double convertedValue = ConvertFromBase(
                    baseValue,
                    targetUnit,
                    model.MeasurementType
                );

                // Step 4: Save to repository
                var entity = new QuantityMeasurementEntity(
                    model.ToString(),
                    QuantityMeasurementEntity.Operations.CONVERT,
                    $"{convertedValue:F2} {targetUnit}",
                    model.MeasurementType
                )
                { UserId = userId };
                _repository.Save(entity);

                // Step 5: Return result as QuantityDTO
                return new QuantityDTO(
                    convertedValue,
                    targetUnit,
                    model.MeasurementType
                );
            }
            catch (QuantityMeasurementException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException(
                    ex.Message,
                    QuantityMeasurementException.ErrorCodes.INVALID_VALUE,
                    ex
                );
            }
        }

        // ─── Add ──────────────────────────────────────────────

        public QuantityDTO Add(QuantityDTO first, QuantityDTO second, int? userId = null)
        {
            return Add(first, second, first.Unit);
        }

        public QuantityDTO Add(QuantityDTO first, QuantityDTO second, string targetUnit, int? userId = null)
        {
            try
            {
                // Step 1: Validate DTOs
                ValidateDTO(first,  "First quantity");
                ValidateDTO(second, "Second quantity");

                // Step 2: Convert DTO -> QuantityModel
                QuantityModel model1 = ToModel(first);
                QuantityModel model2 = ToModel(second);

                // Step 3: Validate
                ValidateSameCategory(model1, model2);
                ValidateArithmeticSupport(model1);

                // Step 4: Add base values
                double base1      = ConvertToBase(model1);
                double base2      = ConvertToBase(model2);
                double result     = base1 + base2;
                double finalValue = ConvertFromBase(
                    result,
                    targetUnit,
                    model1.MeasurementType
                );

                // Step 5: Save to repository
                var entity = new QuantityMeasurementEntity(
                    model1.ToString(),
                    model2.ToString(),
                    QuantityMeasurementEntity.Operations.ADD,
                    $"{finalValue:F2} {targetUnit}",
                    model1.MeasurementType
                )
                { UserId = userId };
                _repository.Save(entity);

                // Step 6: Return result as QuantityDTO
                return new QuantityDTO(
                    finalValue,
                    targetUnit,
                    model1.MeasurementType
                );
            }
            catch (QuantityMeasurementException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException(
                    ex.Message,
                    QuantityMeasurementException.ErrorCodes.INVALID_VALUE,
                    ex
                );
            }
        }

        // ─── Subtract ─────────────────────────────────────────

        public QuantityDTO Subtract(QuantityDTO first, QuantityDTO second, int? userId = null)
        {
            try
            {
                // Step 1: Validate DTOs
                ValidateDTO(first,  "First quantity");
                ValidateDTO(second, "Second quantity");

                // Step 2: Convert DTO -> QuantityModel
                QuantityModel model1 = ToModel(first);
                QuantityModel model2 = ToModel(second);

                // Step 3: Validate
                ValidateSameCategory(model1, model2);
                ValidateArithmeticSupport(model1);

                // Step 4: Subtract base values
                double base1      = ConvertToBase(model1);
                double base2      = ConvertToBase(model2);
                double result     = base1 - base2;
                double finalValue = ConvertFromBase(
                    result,
                    model1.Unit,
                    model1.MeasurementType
                );

                finalValue = Math.Round(finalValue, 2);

                // Step 5: Save to repository
                var entity = new QuantityMeasurementEntity(
                    model1.ToString(),
                    model2.ToString(),
                    QuantityMeasurementEntity.Operations.SUBTRACT,
                    $"{finalValue:F2} {model1.Unit}",
                    model1.MeasurementType
                )
                { UserId = userId };
                _repository.Save(entity);

                // Step 6: Return result as QuantityDTO
                return new QuantityDTO(
                    finalValue,
                    model1.Unit,
                    model1.MeasurementType
                );
                
            }
            catch (QuantityMeasurementException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException(
                    ex.Message,
                    QuantityMeasurementException.ErrorCodes.INVALID_VALUE,
                    ex
                );
            }
        }

        // ─── Divide ───────────────────────────────────────────

        public QuantityDTO Divide(QuantityDTO first, QuantityDTO second, int? userId = null)
        {
            try
            {
                // Step 1: Validate DTOs
                ValidateDTO(first,  "First quantity");
                ValidateDTO(second, "Second quantity");

                // Step 2: Convert DTO -> QuantityModel
                QuantityModel model1 = ToModel(first);
                QuantityModel model2 = ToModel(second);

                // Step 3: Validate
                ValidateSameCategory(model1, model2);
                ValidateArithmeticSupport(model1);

                // Step 4: Divide base values
                double base1 = ConvertToBase(model1);
                double base2 = ConvertToBase(model2);

                if (Math.Abs(base2) < 0.0001)
                    throw new QuantityMeasurementException(
                        "Division by zero is not allowed.",
                        QuantityMeasurementException.ErrorCodes.DIVISION_BY_ZERO
                    );

                double result = base1 / base2;

                // Step 5: Save to repository
                var entity = new QuantityMeasurementEntity(
                    model1.ToString(),
                    model2.ToString(),
                    QuantityMeasurementEntity.Operations.DIVIDE,
                    $"{result:F2}",
                    model1.MeasurementType
                )
                { UserId = userId };
                _repository.Save(entity);

                // Step 6: Return scalar result as QuantityDTO
                return new QuantityDTO(result, "SCALAR", "Result");
            }
            catch (QuantityMeasurementException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new QuantityMeasurementException(
                    ex.Message,
                    QuantityMeasurementException.ErrorCodes.INVALID_VALUE,
                    ex
                );
            }
        }

        // ─── DTO to Model Conversion ──────────────────────────

        /// <summary>
        /// Converts QuantityDTO to QuantityModel for internal use.
        /// This is the key bridge between external and internal representation.
        /// </summary>
        private QuantityModel ToModel(QuantityDTO DTO)
        {
            return new QuantityModel(
                DTO.Value,
                DTO.Unit,
                DTO.MeasurementType
            );
        }

        /// <summary>
        /// Converts QuantityModel back to QuantityDTO for output.
        /// </summary>
        private QuantityDTO ToDTO(QuantityModel model)
        {
            return new QuantityDTO(
                model.Value,
                model.Unit,
                model.MeasurementType
            );
        }

        // ─── Validation Helpers ───────────────────────────────

        private void ValidateDTO(QuantityDTO DTO, string label)
        {
            if (DTO == null)
                throw new QuantityMeasurementException(
                    $"{label} cannot be null.",
                    QuantityMeasurementException.ErrorCodes.NULL_QUANTITY
                );

            if (string.IsNullOrWhiteSpace(DTO.Unit))
                throw new QuantityMeasurementException(
                    $"{label} unit cannot be empty.",
                    QuantityMeasurementException.ErrorCodes.INVALID_UNIT
                );

            if (string.IsNullOrWhiteSpace(DTO.MeasurementType))
                throw new QuantityMeasurementException(
                    $"{label} measurement type cannot be empty.",
                    QuantityMeasurementException.ErrorCodes.INVALID_CATEGORY
                );

            if (double.IsNaN(DTO.Value) || double.IsInfinity(DTO.Value))
                throw new QuantityMeasurementException(
                    $"{label} has invalid numeric value.",
                    QuantityMeasurementException.ErrorCodes.INVALID_VALUE
                );
        }

        private void ValidateSameCategory(QuantityModel m1, QuantityModel m2)
        {
            if (!m1.MeasurementType.Equals(
                    m2.MeasurementType,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new QuantityMeasurementException(
                    $"Cannot perform operation between " +
                    $"{m1.MeasurementType} and {m2.MeasurementType}.",
                    QuantityMeasurementException.ErrorCodes.CROSS_CATEGORY
                );
            }
        }

        private void ValidateArithmeticSupport(QuantityModel model)
        {
            if (model.MeasurementType.Equals(
                    MeasurementTypeDTO.TEMPERATURE,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new QuantityMeasurementException(
                    "Temperature does not support arithmetic operations.",
                    QuantityMeasurementException.ErrorCodes.UNSUPPORTED_OPERATION
                );
            }
        }

        // ─── Conversion Helpers ───────────────────────────────

        /// <summary>
        /// Converts QuantityModel value to base unit.
        /// Uses QuantityModel internally.
        /// </summary>
        private double ConvertToBase(QuantityModel model)
        {
            return model.MeasurementType.ToUpper() switch
            {
                "LENGTH" => model.Unit.ToUpper() switch
                {
                    "FEET"        => model.Value * 1.0,
                    "INCHES"      => model.Value * (1.0 / 12),
                    "YARDS"       => model.Value * 3.0,
                    "CENTIMETERS" => model.Value * (1.0 / 30.48),
                    _ => throw new QuantityMeasurementException(
                        $"Unknown length unit: {model.Unit}",
                        QuantityMeasurementException.ErrorCodes.INVALID_UNIT)
                },
                "WEIGHT" => model.Unit.ToUpper() switch
                {
                    "KILOGRAM" => model.Value * 1.0,
                    "GRAM"     => model.Value * 0.001,
                    "POUND"    => model.Value * 0.453592,
                    _ => throw new QuantityMeasurementException(
                        $"Unknown weight unit: {model.Unit}",
                        QuantityMeasurementException.ErrorCodes.INVALID_UNIT)
                },
                "VOLUME" => model.Unit.ToUpper() switch
                {
                    "LITRE"      => model.Value * 1.0,
                    "MILLILITRE" => model.Value * 0.001,
                    "GALLON"     => model.Value * 3.78541,
                    _ => throw new QuantityMeasurementException(
                        $"Unknown volume unit: {model.Unit}",
                        QuantityMeasurementException.ErrorCodes.INVALID_UNIT)
                },
                "TEMPERATURE" => model.Unit.ToUpper() switch
                {
                    "CELSIUS"    => model.Value,
                    "FAHRENHEIT" => (model.Value - 32) * 5.0 / 9,
                    "KELVIN"     => model.Value - 273.15,
                    _ => throw new QuantityMeasurementException(
                        $"Unknown temperature unit: {model.Unit}",
                        QuantityMeasurementException.ErrorCodes.INVALID_UNIT)
                },
                _ => throw new QuantityMeasurementException(
                    $"Unknown measurement type: {model.MeasurementType}",
                    QuantityMeasurementException.ErrorCodes.INVALID_CATEGORY)
            };
        }

        private double ConvertFromBase(
            double baseValue,
            string targetUnit,
            string measurementType)
        {
            return measurementType.ToUpper() switch
            {
                "LENGTH" => targetUnit.ToUpper() switch
                {
                    "FEET"        => baseValue / 1.0,
                    "INCHES"      => baseValue / (1.0 / 12),
                    "YARDS"       => baseValue / 3.0,
                    "CENTIMETERS" => baseValue / (1.0 / 30.48),
                    _ => throw new QuantityMeasurementException(
                        $"Unknown length unit: {targetUnit}",
                        QuantityMeasurementException.ErrorCodes.INVALID_UNIT)
                },
                "WEIGHT" => targetUnit.ToUpper() switch
                {
                    "KILOGRAM" => baseValue / 1.0,
                    "GRAM"     => baseValue / 0.001,
                    "POUND"    => baseValue / 0.453592,
                    _ => throw new QuantityMeasurementException(
                        $"Unknown weight unit: {targetUnit}",
                        QuantityMeasurementException.ErrorCodes.INVALID_UNIT)
                },
                "VOLUME" => targetUnit.ToUpper() switch
                {
                    "LITRE"      => baseValue / 1.0,
                    "MILLILITRE" => baseValue / 0.001,
                    "GALLON"     => baseValue / 3.78541,
                    _ => throw new QuantityMeasurementException(
                        $"Unknown volume unit: {targetUnit}",
                        QuantityMeasurementException.ErrorCodes.INVALID_UNIT)
                },
                "TEMPERATURE" => targetUnit.ToUpper() switch
                {
                    "CELSIUS"    => baseValue,
                    "FAHRENHEIT" => (baseValue * 9.0 / 5) + 32,
                    "KELVIN"     => baseValue + 273.15,
                    _ => throw new QuantityMeasurementException(
                        $"Unknown temperature unit: {targetUnit}",
                        QuantityMeasurementException.ErrorCodes.INVALID_UNIT)
                },
                _ => throw new QuantityMeasurementException(
                    $"Unknown measurement type: {measurementType}",
                    QuantityMeasurementException.ErrorCodes.INVALID_CATEGORY)
            };
        }
    }
}