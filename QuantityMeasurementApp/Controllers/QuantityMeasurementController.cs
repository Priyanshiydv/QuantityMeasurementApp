using System;
using QuantityMeasurement.Models.DTOs;
using QuantityMeasurement.Models.Exceptions;
using QuantityMeasurement.Service.Interfaces;

namespace QuantityMeasurementApp.Controllers
{
    /// <summary>
    /// Controller layer for Quantity Measurement Application.
    /// Handles user interaction and delegates to service layer.
    /// UC15
    /// </summary>
    public class QuantityMeasurementController
    {
        // ─── Service Dependency ───────────────────────────────

        private readonly IQuantityMeasurementService _service;

        // ─── Constructor (Dependency Injection) ───────────────

        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            _service = service;
        }

        // ─── Compare ──────────────────────────────────────────

        /// <summary>
        /// Performs equality comparison between two quantities.
        /// </summary>
        public void PerformComparison(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                Console.WriteLine("\n--- Comparison ---");
                Console.WriteLine($"First  : {first}");
                Console.WriteLine($"Second : {second}");

                QuantityDTO result = _service.Compare(first, second);

                Console.WriteLine($"Result : {result.Unit}");
            }
            catch (QuantityMeasurementException ex)
            {
                DisplayError(ex);
            }
        }

        // ─── Convert ──────────────────────────────────────────

        /// <summary>
        /// Performs unit conversion on a quantity.
        /// </summary>
        public void PerformConversion(QuantityDTO quantity, string targetUnit)
        {
            try
            {
                Console.WriteLine("\n--- Conversion ---");
                Console.WriteLine($"Input      : {quantity}");
                Console.WriteLine($"Target Unit: {targetUnit}");

                QuantityDTO result = _service.Convert(quantity, targetUnit);

                Console.WriteLine($"Result     : {result.Value:F2} {result.Unit}");
            }
            catch (QuantityMeasurementException ex)
            {
                DisplayError(ex);
            }
        }

        // ─── Add ──────────────────────────────────────────────

        /// <summary>
        /// Performs addition of two quantities.
        /// Result in first quantity's unit.
        /// </summary>
        public void PerformAddition(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                Console.WriteLine("\n--- Addition ---");
                Console.WriteLine($"First  : {first}");
                Console.WriteLine($"Second : {second}");

                QuantityDTO result = _service.Add(first, second);

                Console.WriteLine($"Result : {result.Value:F2} {result.Unit}");
            }
            catch (QuantityMeasurementException ex)
            {
                DisplayError(ex);
            }
        }

        /// <summary>
        /// Performs addition of two quantities in target unit.
        /// </summary>
        public void PerformAddition(
            QuantityDTO first,
            QuantityDTO second,
            string targetUnit)
        {
            try
            {
                Console.WriteLine("\n--- Addition ---");
                Console.WriteLine($"First       : {first}");
                Console.WriteLine($"Second      : {second}");
                Console.WriteLine($"Target Unit : {targetUnit}");

                QuantityDTO result = _service.Add(first, second, targetUnit);

                Console.WriteLine($"Result      : {result.Value:F2} {result.Unit}");
            }
            catch (QuantityMeasurementException ex)
            {
                DisplayError(ex);
            }
        }

        // ─── Subtract ─────────────────────────────────────────

        /// <summary>
        /// Performs subtraction of two quantities.
        /// </summary>
        public void PerformSubtraction(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                Console.WriteLine("\n--- Subtraction ---");
                Console.WriteLine($"First  : {first}");
                Console.WriteLine($"Second : {second}");

                QuantityDTO result = _service.Subtract(first, second);

                Console.WriteLine($"Result : {result.Value:F2} {result.Unit}");
            }
            catch (QuantityMeasurementException ex)
            {
                DisplayError(ex);
            }
        }

        // ─── Divide ───────────────────────────────────────────

        /// <summary>
        /// Performs division of two quantities.
        /// </summary>
        public void PerformDivision(QuantityDTO first, QuantityDTO second)
        {
            try
            {
                Console.WriteLine("\n--- Division ---");
                Console.WriteLine($"First  : {first}");
                Console.WriteLine($"Second : {second}");

                QuantityDTO result = _service.Divide(first, second);

                Console.WriteLine($"Result : {result.Value:F2}");
            }
            catch (QuantityMeasurementException ex)
            {
                DisplayError(ex);
            }
        }

        // ─── Display All Operations ───────────────────────────

        /// <summary>
        /// Runs all demonstration operations for UC15.
        /// </summary>
        public void RunAllDemonstrations()
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine("   QUANTITY MEASUREMENT APP - UC15");
            Console.WriteLine("========================================");

            // Length Comparison
            PerformComparison(
                new QuantityDTO(1, LengthUnitDTO.FEET,
                    MeasurementTypeDTO.LENGTH),
                new QuantityDTO(12, LengthUnitDTO.INCHES,
                    MeasurementTypeDTO.LENGTH)
            );

            // Length Conversion
            PerformConversion(
                new QuantityDTO(1, LengthUnitDTO.FEET,
                    MeasurementTypeDTO.LENGTH),
                LengthUnitDTO.INCHES
            );

            // Length Addition
            PerformAddition(
                new QuantityDTO(1, LengthUnitDTO.FEET,
                    MeasurementTypeDTO.LENGTH),
                new QuantityDTO(12, LengthUnitDTO.INCHES,
                    MeasurementTypeDTO.LENGTH)
            );

            // Weight Subtraction
            PerformSubtraction(
                new QuantityDTO(5, WeightUnitDTO.KILOGRAM,
                    MeasurementTypeDTO.WEIGHT),
                new QuantityDTO(500, WeightUnitDTO.GRAM,
                    MeasurementTypeDTO.WEIGHT)
            );

            // Volume Division
            PerformDivision(
                new QuantityDTO(2, VolumeUnitDTO.GALLON,
                    MeasurementTypeDTO.VOLUME),
                new QuantityDTO(1, VolumeUnitDTO.GALLON,
                    MeasurementTypeDTO.VOLUME)
            );

            // Temperature Arithmetic (should show error)
            PerformAddition(
                new QuantityDTO(100, TemperatureUnitDTO.CELSIUS,
                    MeasurementTypeDTO.TEMPERATURE),
                new QuantityDTO(50, TemperatureUnitDTO.CELSIUS,
                    MeasurementTypeDTO.TEMPERATURE)
            );

            // Cross Category (should show error)
            PerformComparison(
                new QuantityDTO(1, LengthUnitDTO.FEET,
                    MeasurementTypeDTO.LENGTH),
                new QuantityDTO(1, WeightUnitDTO.KILOGRAM,
                    MeasurementTypeDTO.WEIGHT)
            );

            Console.WriteLine("\n========================================");
            Console.WriteLine("   DEMONSTRATIONS COMPLETE");
            Console.WriteLine("========================================");
        }

        // ─── Private Helper ───────────────────────────────────

        /// <summary>
        /// Displays error message in consistent format.
        /// </summary>
        private void DisplayError(QuantityMeasurementException ex)
        {
            Console.WriteLine($"Error [{ex.ErrorCode}]: {ex.Message}");
        }
    }
}