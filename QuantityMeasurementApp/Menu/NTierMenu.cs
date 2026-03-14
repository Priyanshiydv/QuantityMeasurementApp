using System;
using QuantityMeasurement.Models.DTOs;
using QuantityMeasurement.Models.Exceptions;
using QuantityMeasurement.Repository.Service;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Repository.Utilities;
using QuantityMeasurement.Service.Service;
using QuantityMeasurementApp.Controllers;

namespace QuantityMeasurementApp.Menu
{
    /// <summary>
    /// Menu driven application for UC15.
    /// Uses Controller -> Service -> Repository layers.
    /// Updated in UC16 to support both Cache and Database
    /// repositories based on appsettings.json configuration.
    /// UC15, UC16
    /// </summary>
    public class NTierMenu
    {
        /// <summary>
        /// Controller for handling all quantity operations.
        /// </summary>
        private static QuantityMeasurementController? _controller;

        /// <summary>
        /// Repository reference.
        /// Added in UC16 for configuration based switching.
        /// </summary>
        private static IQuantityMeasurementRepository? _repository;

        // ─── Initialize ───────────────────────────────────────

        public void Initialize()
        {
            // Step 1: Load application configuration
            // Reads appsettings.json to determine repository type
            ApplicationConfig config =
                ApplicationConfig.GetInstance();

            Console.WriteLine(
                $"\n[NTierMenu] Initializing..." +
                $"\n[NTierMenu] Environment  : {config.GetEnvironment()}" +
                $"\n[NTierMenu] Repository   : {config.GetRepositoryType()}");

            // Step 2: Create repository based on configuration
            // If "database" in appsettings.json → use DatabaseRepository
            // If "cache"    in appsettings.json → use CacheRepository
            if (config.IsDatabaseRepository())
            {
                // Create connection pool (Singleton)
                ConnectionPool connectionPool =
                    ConnectionPool.GetInstance();

                // Create database repository with connection pool
                _repository =
                    new QuantityMeasurementDatabaseRepository(
                        connectionPool,
                        "QuantityMeasurementDB");

                Console.WriteLine(
                    "[NTierMenu] Using Database Repository ✓");
            }
            else
            {
                // Fallback to in-memory cache repository
                _repository =
                    QuantityMeasurementCacheRepository.GetInstance();

                Console.WriteLine(
                    "[NTierMenu] Using Cache Repository ✓");
            }
            // Step 3: Create Service (Dependency Injection)
            // Service does not know which repository is injected
            QuantityMeasurementServiceImpl service =
                new QuantityMeasurementServiceImpl(_repository);

            // Step 4: Create Controller (Dependency Injection)
            // Controller does not know which service is injected
            _controller =
                new QuantityMeasurementController(service);

            Console.WriteLine(
                "[NTierMenu] Initialization complete ✓");
        }

        // ─── Main Menu ────────────────────────────────────────

        public void Start()
        {
            Initialize();

            while (true)
            {
                Console.WriteLine("\n========================================");
                Console.WriteLine("   QUANTITY MEASUREMENT APP - UC15");
                Console.WriteLine($"   Repository: " + $"{ApplicationConfig.GetInstance().GetRepositoryType().ToUpper()}");
                Console.WriteLine("========================================");
                Console.WriteLine("1.  Length Operations");
                Console.WriteLine("2.  Weight Operations");
                Console.WriteLine("3.  Volume Operations");
                Console.WriteLine("4.  Temperature Operations");
                Console.WriteLine("5.  Cross Category Operations");
                Console.WriteLine("6.  Run All Demonstrations");
                Console.WriteLine("7.  View All History"); //UC16
                Console.WriteLine("8.  View By Operation Type");
                Console.WriteLine("9.  View By Measurement Type");
                Console.WriteLine("10. View Statistics");
                Console.WriteLine("11. Clear All Records");
                Console.WriteLine("0.  Exit ");
                Console.WriteLine("========================================");
                Console.Write("Select Option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": LengthMenu();           break;
                    case "2": WeightMenu();           break;
                    case "3": VolumeMenu();           break;
                    case "4": TemperatureMenu();      break;
                    case "5": CrossCategoryMenu();    break;
                    case "6": _controller!
                                .RunAllDemonstrations(); break;
                    case "7":  ViewAllHistory();         break;
                    case "8":  ViewByOperationType();    break;
                    case "9":  ViewByMeasurementType();  break;
                    case "10": ViewStatistics();         break;
                    case "11": ClearAllRecords();        break;
                    case "0": 
                          // Release resources on exit
                        _repository!.ReleaseResources();
                        Console.WriteLine(
                            "[NTierMenu] Resources released. " +
                            "Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        // ─── Length Menu ──────────────────────────────────────

        private static void LengthMenu()
        {
            Console.WriteLine("\n--- Length Operations ---");
            Console.WriteLine("1. Compare");
            Console.WriteLine("2. Convert");
            Console.WriteLine("3. Add");
            Console.WriteLine("4. Subtract");
            Console.WriteLine("5. Divide");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var f1 = ReadLength("First Length");
                    var f2 = ReadLength("Second Length");
                    _controller!.PerformComparison(f1, f2);
                    break;

                case "2":
                    var q = ReadLength("Length to Convert");
                    Console.WriteLine("Target Unit:");
                    string target = ReadLengthUnitName();
                    _controller!.PerformConversion(q, target);
                    break;

                case "3":
                    var a1 = ReadLength("First Length");
                    var a2 = ReadLength("Second Length");
                    Console.WriteLine("1. Implicit Unit");
                    Console.WriteLine("2. Explicit Unit");
                    string? addChoice = Console.ReadLine();
                    if (addChoice == "2")
                    {
                        string tUnit = ReadLengthUnitName();
                        _controller!.PerformAddition(a1, a2, tUnit);
                    }
                    else
                        _controller!.PerformAddition(a1, a2);
                    break;

                case "4":
                    var s1 = ReadLength("First Length");
                    var s2 = ReadLength("Second Length");
                    _controller!.PerformSubtraction(s1, s2);
                    break;

                case "5":
                    var d1 = ReadLength("First Length");
                    var d2 = ReadLength("Second Length");
                    _controller!.PerformDivision(d1, d2);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        // ─── Weight Menu ──────────────────────────────────────

        private static void WeightMenu()
        {
            Console.WriteLine("\n--- Weight Operations ---");
            Console.WriteLine("1. Compare");
            Console.WriteLine("2. Convert");
            Console.WriteLine("3. Add");
            Console.WriteLine("4. Subtract");
            Console.WriteLine("5. Divide");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var f1 = ReadWeight("First Weight");
                    var f2 = ReadWeight("Second Weight");
                    _controller!.PerformComparison(f1, f2);
                    break;

                case "2":
                    var q = ReadWeight("Weight to Convert");
                    Console.WriteLine("Target Unit:");
                    string target = ReadWeightUnitName();
                    _controller!.PerformConversion(q, target);
                    break;

                case "3":
                    var a1 = ReadWeight("First Weight");
                    var a2 = ReadWeight("Second Weight");
                    Console.WriteLine("1. Implicit Unit");
                    Console.WriteLine("2. Explicit Unit");
                    string? addChoice = Console.ReadLine();
                    if (addChoice == "2")
                    {
                        string tUnit = ReadWeightUnitName();
                        _controller!.PerformAddition(a1, a2, tUnit);
                    }
                    else
                        _controller!.PerformAddition(a1, a2);
                    break;

                case "4":
                    var s1 = ReadWeight("First Weight");
                    var s2 = ReadWeight("Second Weight");
                    _controller!.PerformSubtraction(s1, s2);
                    break;

                case "5":
                    var d1 = ReadWeight("First Weight");
                    var d2 = ReadWeight("Second Weight");
                    _controller!.PerformDivision(d1, d2);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        // ─── Volume Menu ──────────────────────────────────────

        private static void VolumeMenu()
        {
            Console.WriteLine("\n--- Volume Operations ---");
            Console.WriteLine("1. Compare");
            Console.WriteLine("2. Convert");
            Console.WriteLine("3. Add");
            Console.WriteLine("4. Subtract");
            Console.WriteLine("5. Divide");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var f1 = ReadVolume("First Volume");
                    var f2 = ReadVolume("Second Volume");
                    _controller!.PerformComparison(f1, f2);
                    break;

                case "2":
                    var q = ReadVolume("Volume to Convert");
                    Console.WriteLine("Target Unit:");
                    string target = ReadVolumeUnitName();
                    _controller!.PerformConversion(q, target);
                    break;

                case "3":
                    var a1 = ReadVolume("First Volume");
                    var a2 = ReadVolume("Second Volume");
                    Console.WriteLine("1. Implicit Unit");
                    Console.WriteLine("2. Explicit Unit");
                    string? addChoice = Console.ReadLine();
                    if (addChoice == "2")
                    {
                        string tUnit = ReadVolumeUnitName();
                        _controller!.PerformAddition(a1, a2, tUnit);
                    }
                    else
                        _controller!.PerformAddition(a1, a2);
                    break;

                case "4":
                    var s1 = ReadVolume("First Volume");
                    var s2 = ReadVolume("Second Volume");
                    _controller!.PerformSubtraction(s1, s2);
                    break;

                case "5":
                    var d1 = ReadVolume("First Volume");
                    var d2 = ReadVolume("Second Volume");
                    _controller!.PerformDivision(d1, d2);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        // ─── Temperature Menu ─────────────────────────────────

        private static void TemperatureMenu()
        {
            Console.WriteLine("\n--- Temperature Operations ---");
            Console.WriteLine("1. Compare");
            Console.WriteLine("2. Convert");
            Console.WriteLine("3. Add (will show error)");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var f1 = ReadTemperature("First Temperature");
                    var f2 = ReadTemperature("Second Temperature");
                    _controller!.PerformComparison(f1, f2);
                    break;

                case "2":
                    var q = ReadTemperature("Temperature to Convert");
                    Console.WriteLine("Target Unit:");
                    string target = ReadTemperatureUnitName();
                    _controller!.PerformConversion(q, target);
                    break;

                case "3":
                    var a1 = ReadTemperature("First Temperature");
                    var a2 = ReadTemperature("Second Temperature");
                    _controller!.PerformAddition(a1, a2);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        // ─── Cross Category Menu ──────────────────────────────

        private static void CrossCategoryMenu()
        {
            Console.WriteLine("\n--- Cross Category Operations ---");
            Console.WriteLine("(These will show errors as expected)");

            Console.WriteLine("\nSelect First Quantity:");
            var q1 = ReadAnyQuantity();

            Console.WriteLine("\nSelect Second Quantity:");
            var q2 = ReadAnyQuantity();

            Console.WriteLine("\n1. Compare");
            Console.WriteLine("2. Add");
            Console.Write("Choice: ");

            string? op = Console.ReadLine();

            switch (op)
            {
                case "1":
                    _controller!.PerformComparison(q1, q2);
                    break;
                case "2":
                    _controller!.PerformAddition(q1, q2);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        // ─── Read Helpers ─────────────────────────────────────

        private static QuantityDTO ReadLength(string label)
        {
            Console.WriteLine($"\n{label}:");
            Console.Write("Enter Value: ");
            double value = Convert.ToDouble(Console.ReadLine());
            string unit  = ReadLengthUnitName();
            return new QuantityDTO(value, unit,
                MeasurementTypeDTO.LENGTH);
        }

        private static string ReadLengthUnitName()
        {
            Console.WriteLine("1. FEET");
            Console.WriteLine("2. INCHES");
            Console.WriteLine("3. YARDS");
            Console.WriteLine("4. CENTIMETERS");
            Console.Write("Choice: ");
            return Console.ReadLine() switch
            {
                "1" => LengthUnitDTO.FEET,
                "2" => LengthUnitDTO.INCHES,
                "3" => LengthUnitDTO.YARDS,
                "4" => LengthUnitDTO.CENTIMETERS,
                _   => throw new ArgumentException("Invalid unit")
            };
        }

        private static QuantityDTO ReadWeight(string label)
        {
            Console.WriteLine($"\n{label}:");
            Console.Write("Enter Value: ");
            double value = Convert.ToDouble(Console.ReadLine());
            string unit  = ReadWeightUnitName();
            return new QuantityDTO(value, unit,
                MeasurementTypeDTO.WEIGHT);
        }

        private static string ReadWeightUnitName()
        {
            Console.WriteLine("1. KILOGRAM");
            Console.WriteLine("2. GRAM");
            Console.WriteLine("3. POUND");
            Console.Write("Choice: ");
            return Console.ReadLine() switch
            {
                "1" => WeightUnitDTO.KILOGRAM,
                "2" => WeightUnitDTO.GRAM,
                "3" => WeightUnitDTO.POUND,
                _   => throw new ArgumentException("Invalid unit")
            };
        }

        private static QuantityDTO ReadVolume(string label)
        {
            Console.WriteLine($"\n{label}:");
            Console.Write("Enter Value: ");
            double value = Convert.ToDouble(Console.ReadLine());
            string unit  = ReadVolumeUnitName();
            return new QuantityDTO(value, unit,
                MeasurementTypeDTO.VOLUME);
        }

        private static string ReadVolumeUnitName()
        {
            Console.WriteLine("1. LITRE");
            Console.WriteLine("2. MILLILITRE");
            Console.WriteLine("3. GALLON");
            Console.Write("Choice: ");
            return Console.ReadLine() switch
            {
                "1" => VolumeUnitDTO.LITRE,
                "2" => VolumeUnitDTO.MILLILITRE,
                "3" => VolumeUnitDTO.GALLON,
                _   => throw new ArgumentException("Invalid unit")
            };
        }

        private static QuantityDTO ReadTemperature(string label)
        {
            Console.WriteLine($"\n{label}:");
            Console.Write("Enter Value: ");
            double value = Convert.ToDouble(Console.ReadLine());
            string unit  = ReadTemperatureUnitName();
            return new QuantityDTO(value, unit,
                MeasurementTypeDTO.TEMPERATURE);
        }

        private static string ReadTemperatureUnitName()
        {
            Console.WriteLine("1. CELSIUS");
            Console.WriteLine("2. FAHRENHEIT");
            Console.WriteLine("3. KELVIN");
            Console.Write("Choice: ");
            return Console.ReadLine() switch
            {
                "1" => TemperatureUnitDTO.CELSIUS,
                "2" => TemperatureUnitDTO.FAHRENHEIT,
                "3" => TemperatureUnitDTO.KELVIN,
                _   => throw new ArgumentException("Invalid unit")
            };
        }

        private static QuantityDTO ReadAnyQuantity()
        {
            Console.WriteLine("1. Length");
            Console.WriteLine("2. Weight");
            Console.WriteLine("3. Volume");
            Console.WriteLine("4. Temperature");
            Console.Write("Choice: ");

            return Console.ReadLine() switch
            {
                "1" => ReadLength("Length"),
                "2" => ReadWeight("Weight"),
                "3" => ReadVolume("Volume"),
                "4" => ReadTemperature("Temperature"),
                _   => throw new ArgumentException("Invalid category")
            };
        }

        // ─── UC16 History Menu Options ────────────────────────

        /// <summary>
        /// Displays all operation history from repository.
        /// Works for both cache and database repository.
        /// </summary>
        private static void ViewAllHistory()
        {
            Console.WriteLine(
                "\n--- All Measurement History ---");

            var measurements =
                _repository!.GetAllMeasurements();

            if (measurements.Count == 0)
            {
                Console.WriteLine("No records found.");
                return;
            }

            Console.WriteLine(
                $"Total Records: {measurements.Count}\n");

            foreach (var entity in measurements)
            {
                Console.WriteLine(entity.ToString());
            }
        }

        /// <summary>
        /// Displays measurements filtered by operation type.
        /// e.g. COMPARE, ADD, SUBTRACT, DIVIDE, CONVERT
        /// </summary>
        private static void ViewByOperationType()
        {
            Console.WriteLine(
                "\n--- View By Operation Type ---");
            Console.WriteLine("1. COMPARE");
            Console.WriteLine("2. CONVERT");
            Console.WriteLine("3. ADD");
            Console.WriteLine("4. SUBTRACT");
            Console.WriteLine("5. DIVIDE");
            Console.Write("Choice: ");

            string operationType = Console.ReadLine() switch
            {
                "1" => "COMPARE",
                "2" => "CONVERT",
                "3" => "ADD",
                "4" => "SUBTRACT",
                "5" => "DIVIDE",
                _   => "COMPARE"
            };

            var measurements =
                _repository!.GetMeasurementsByOperationType(
                    operationType);

            Console.WriteLine(
                $"\n{operationType} Records: " +
                $"{measurements.Count}\n");

            foreach (var entity in measurements)
            {
                Console.WriteLine(entity.ToString());
            }
        }

        /// <summary>
        /// Displays measurements filtered by measurement type.
        /// e.g. Length, Weight, Volume, Temperature
        /// </summary>
        private static void ViewByMeasurementType()
        {
            Console.WriteLine(
                "\n--- View By Measurement Type ---");
            Console.WriteLine("1. Length");
            Console.WriteLine("2. Weight");
            Console.WriteLine("3. Volume");
            Console.WriteLine("4. Temperature");
            Console.Write("Choice: ");

            string measurementType = Console.ReadLine() switch
            {
                "1" => "Length",
                "2" => "Weight",
                "3" => "Volume",
                "4" => "Temperature",
                _   => "Length"
            };

            var measurements =
                _repository!.GetMeasurementsByMeasurementType(
                    measurementType);

            Console.WriteLine(
                $"\n{measurementType} Records: " +
                $"{measurements.Count}\n");

            foreach (var entity in measurements)
            {
                Console.WriteLine(entity.ToString());
            }
        }

        /// <summary>
        /// Displays repository statistics.
        /// Shows pool stats for database, cache stats for cache.
        /// </summary>
        private static void ViewStatistics()
        {
            Console.WriteLine(
                "\n--- Repository Statistics ---");
            Console.WriteLine(
                _repository!.GetPoolStatistics());
            Console.WriteLine(
                $"Total Records: " +
                $"{_repository.GetTotalCount()}");
        }

        /// <summary>
        /// Clears all records from repository.
        /// Asks for confirmation before deleting.
        /// </summary>
        private static void ClearAllRecords()
        {
            Console.WriteLine(
                "\n--- Clear All Records ---");
            Console.Write(
                "Are you sure? (yes/no): ");

            string? confirm = Console.ReadLine();

            if (confirm?.ToLower() == "yes")
            {
                int deleted =
                    _repository!.DeleteAllMeasurements();

                Console.WriteLine(
                    $"Deleted {deleted} records successfully.");
            }
            else
            {
                Console.WriteLine("Operation cancelled.");
            }
        }

    }
}