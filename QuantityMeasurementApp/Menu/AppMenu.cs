using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Menu
{
    public static class AppMenu
    {
        private static readonly QuantityMeasurementService service = new QuantityMeasurementService();

        public static void Start()
        {
            while (true)
            {
                Console.WriteLine("\n========== QUANTITY MEASUREMENT APP ==========");
                Console.WriteLine("1  - Length Operations");
                Console.WriteLine("2  - Weight Operations");
                Console.WriteLine("3  - Volume Operations");
                Console.WriteLine("4  - Temperature Operations");
                Console.WriteLine("5  - Subtraction Operation");
                Console.WriteLine("6  - Division Operation");
                Console.WriteLine("7  - Cross Category Equality");
                Console.WriteLine("8  - Cross Category Operations");
                Console.WriteLine("9  - Generic Demonstration");
                Console.WriteLine("0  - Exit");

                Console.Write("\nSelect Option: ");
                string? choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": LengthMenu(); break;
                        case "2": WeightMenu(); break;
                        case "3": VolumeMenu(); break;
                        case "4": TemperatureMenu(); break;
                        case "5": SubtractionDemo(); break;
                        case "6": DivisionDemo(); break;
                        case "7": CrossCategoryEquality(); break;
                        case "8": CrossCategoryOperation(); break;
                        case "9": GenericDemo(); break;
                        case "0":
                            Console.WriteLine("Exiting Application...");
                            return;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        // ================= LENGTH MENU =================

        private static void LengthMenu()
        {
            Console.WriteLine("\n---- LENGTH OPERATIONS ----");
            Console.WriteLine("1 Equality");
            Console.WriteLine("2 Conversion");
            Console.WriteLine("3 Addition");

            switch (Console.ReadLine())
            {
                case "1": LengthEquality(); break;
                case "2": LengthConversion(); break;
                case "3": LengthAddition(); break;
                default: Console.WriteLine("Invalid choice"); break;
            }
        }

        // ================= WEIGHT MENU =================

        private static void WeightMenu()
        {
            Console.WriteLine("\n---- WEIGHT OPERATIONS ----");
            Console.WriteLine("1 Equality");
            Console.WriteLine("2 Conversion");
            Console.WriteLine("3 Addition");

            switch (Console.ReadLine())
            {
                case "1": WeightEquality(); break;
                case "2": WeightConversion(); break;
                case "3": WeightAddition(); break;
                default: Console.WriteLine("Invalid choice"); break;
            }
        }

        // ================= VOLUME MENU =================

        private static void VolumeMenu()
        {
            Console.WriteLine("\n---- VOLUME OPERATIONS ----");
            Console.WriteLine("1 Equality");
            Console.WriteLine("2 Conversion");
            Console.WriteLine("3 Addition");

            switch (Console.ReadLine())
            {
                case "1": VolumeEquality(); break;
                case "2": VolumeConversion(); break;
                case "3": VolumeAddition(); break;
                default: Console.WriteLine("Invalid choice"); break;
            }
        }

        // ================= TEMPERATURE MENU =================

        private static void TemperatureMenu()
        {
            Console.WriteLine("\n---- TEMPERATURE OPERATIONS ----");
            Console.WriteLine("1 Equality");
            Console.WriteLine("2 Conversion");
            Console.WriteLine("3 Arithmetic Test");

            switch (Console.ReadLine())
            {
                case "1": TemperatureEquality(); break;
                case "2": TemperatureConversion(); break;
                case "3": TemperatureArithmeticTest(); break;
                default: Console.WriteLine("Invalid choice"); break;
            }
        }

        // ---------------- LENGTH ----------------

        private static void LengthEquality()
        {
            Console.WriteLine("\nEnter First Length");
            var q1 = ReadLength();

            Console.WriteLine("\nEnter Second Length");
            var q2 = ReadLength();

            Console.WriteLine($"Result: {service.AreEqual(q1, q2)}");
        }

        private static void LengthConversion()
        {
            Console.WriteLine("\nEnter Length");
            var length = ReadLength();

            Console.WriteLine("Convert To Unit:");
            LengthUnit target = ReadLengthUnit();

            Console.WriteLine($"Converted: {length.ConvertTo(target)}");
        }

        private static void LengthAddition()
        {
            var q1 = ReadLength();
            var q2 = ReadLength();

            Console.WriteLine("1. Implicit Unit");
            Console.WriteLine("2. Explicit Unit");

            if (Console.ReadLine() == "1")
                Console.WriteLine($"Result: {q1.Add(q2)}");
            else
                Console.WriteLine($"Result: {q1.Add(q2, ReadLengthUnit())}");
        }

        // ---------------- WEIGHT ----------------

        private static void WeightEquality()
        {
            var w1 = ReadWeight();
            var w2 = ReadWeight();

            Console.WriteLine($"Result: {service.AreEqual(w1, w2)}");
        }

        private static void WeightConversion()
        {
            var weight = ReadWeight();

            Console.WriteLine("Convert To Unit:");
            Console.WriteLine($"Converted: {weight.ConvertTo(ReadWeightUnit())}");
        }

        private static void WeightAddition()
        {
            var w1 = ReadWeight();
            var w2 = ReadWeight();

            Console.WriteLine("1. Implicit Unit");
            Console.WriteLine("2. Explicit Unit");

            if (Console.ReadLine() == "1")
                Console.WriteLine($"Result: {w1.Add(w2)}");
            else
                Console.WriteLine($"Result: {w1.Add(w2, ReadWeightUnit())}");
        }

        // ---------------- VOLUME ----------------

        private static void VolumeEquality()
        {
            var v1 = ReadVolume();
            var v2 = ReadVolume();

            Console.WriteLine($"Result: {service.AreEqual(v1, v2)}");
        }

        private static void VolumeConversion()
        {
            var v = ReadVolume();
            Console.WriteLine($"Converted: {v.ConvertTo(ReadVolumeUnit())}");
        }

        private static void VolumeAddition()
        {
            var v1 = ReadVolume();
            var v2 = ReadVolume();

            Console.WriteLine("1. Implicit Unit");
            Console.WriteLine("2. Explicit Unit");

            if (Console.ReadLine() == "1")
                Console.WriteLine($"Result: {v1.Add(v2)}");
            else
                Console.WriteLine($"Result: {v1.Add(v2, ReadVolumeUnit())}");
        }

        // ---------------- SUBTRACTION ----------------

        private static void SubtractionDemo()
        {
            Console.WriteLine("\nSelect Category");
            Console.WriteLine("1 Length");
            Console.WriteLine("2 Weight");
            Console.WriteLine("3 Volume");

            string? choice = Console.ReadLine();

            dynamic q1 = ReadQuantity(choice);
            dynamic q2 = ReadQuantity(choice);

            Console.WriteLine($"Result: {q1.Subtract(q2)}");
        }

        // ---------------- DIVISION ----------------

        private static void DivisionDemo()
        {
            Console.WriteLine("\nSelect Category");
            Console.WriteLine("1 Length");
            Console.WriteLine("2 Weight");
            Console.WriteLine("3 Volume");

            string? choice = Console.ReadLine();

            dynamic q1 = ReadQuantity(choice);
            dynamic q2 = ReadQuantity(choice);

            Console.WriteLine($"Division Result: {q1.Divide(q2)}");
        }

        // ---------------- GENERIC DEMO ----------------

        private static void GenericDemo()
        {
            Console.WriteLine("1 Length");
            Console.WriteLine("2 Weight");
            Console.WriteLine("3 Volume");

            string? choice = Console.ReadLine();

            if (choice == "1")
                DemonstrateEquality(ReadLength(), ReadLength());
            else if (choice == "2")
                DemonstrateEquality(ReadWeight(), ReadWeight());
            else
                DemonstrateEquality(ReadVolume(), ReadVolume());
        }

        private static void DemonstrateEquality<T>(QuantityGeneric<T> q1, QuantityGeneric<T> q2)
            where T : IMeasurable
        {
            Console.WriteLine($"Q1: {q1}");
            Console.WriteLine($"Q2: {q2}");
            Console.WriteLine($"Equal: {q1.Equals(q2)}");
        }

        // ---------------- CROSS CATEGORY ----------------

        private static void CrossCategoryEquality()
        {
            dynamic q1 = ReadAnyCategoryQuantity("First");
            dynamic q2 = ReadAnyCategoryQuantity("Second");

            if (q1.GetType() != q2.GetType())
                Console.WriteLine("Result: False (Different Categories)");
            else
                Console.WriteLine("Same Category. Use specific equality option.");
        }

        private static void CrossCategoryOperation()
        {
            dynamic q1 = ReadAnyCategoryQuantity("First");
            dynamic q2 = ReadAnyCategoryQuantity("Second");

            Console.WriteLine("1 Subtract");
            Console.WriteLine("2 Divide");

            if (Console.ReadLine() == "1")
                Console.WriteLine($"Result: {q1.Subtract(q2)}");
            else
                Console.WriteLine($"Division Result: {q1.Divide(q2)}");
        }

        // ---------------- TEMPERATURE ----------------

        private static void TemperatureEquality()
        {
            var t1 = ReadTemperature();
            var t2 = ReadTemperature();

            Console.WriteLine($"Result: {t1.Equals(t2)}");
        }

        private static void TemperatureConversion()
        {
            var temp = ReadTemperature();
            var target = new TemperatureUnitWrapper(ReadTemperatureUnit());

            Console.WriteLine($"Converted: {temp.ConvertTo(target)}");
        }

        private static void TemperatureArithmeticTest()
        {
            try
            {
                var t1 = ReadTemperature();
                var t2 = ReadTemperature();

                Console.WriteLine(t1.Add(t2));
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // ================= INPUT METHODS =================

        private static double ReadValue()
        {
            double value;
            while (!double.TryParse(Console.ReadLine(), out value))
                Console.WriteLine("Invalid number. Enter again:");
            return value;
        }

        private static QuantityGeneric<LengthUnit> ReadLength()
        {
            Console.Write("Enter Value: ");
            double value = ReadValue();
            return new QuantityGeneric<LengthUnit>(value, ReadLengthUnit());
        }

        private static QuantityGeneric<WeightUnit> ReadWeight()
        {
            Console.Write("Enter Value: ");
            double value = ReadValue();
            return new QuantityGeneric<WeightUnit>(value, ReadWeightUnit());
        }

        private static QuantityGeneric<VolumeUnit> ReadVolume()
        {
            Console.Write("Enter Value: ");
            double value = ReadValue();
            return new QuantityGeneric<VolumeUnit>(value, ReadVolumeUnit());
        }

        private static QuantityGeneric<TemperatureUnitWrapper> ReadTemperature()
        {
            Console.Write("Enter Value: ");
            double value = ReadValue();
            return new QuantityGeneric<TemperatureUnitWrapper>(value,
                new TemperatureUnitWrapper(ReadTemperatureUnit()));
        }

        private static dynamic ReadQuantity(string? category)
        {
            return category switch
            {
                "1" => ReadLength(),
                "2" => ReadWeight(),
                "3" => ReadVolume(),
                _ => throw new ArgumentException("Invalid category")
            };
        }

        private static dynamic ReadAnyCategoryQuantity(string label)
        {
            Console.WriteLine($"\nSelect {label} Category");
            Console.WriteLine("1 Length");
            Console.WriteLine("2 Weight");
            Console.WriteLine("3 Volume");
            Console.WriteLine("4 Temperature");

            return Console.ReadLine() switch
            {
                "1" => ReadLength(),
                "2" => ReadWeight(),
                "3" => ReadVolume(),
                "4" => ReadTemperature(),
                _ => throw new ArgumentException("Invalid category")
            };
        }

        // ================= UNIT SELECTION =================

        private static LengthUnit ReadLengthUnit()
        {
            Console.WriteLine("1 FEET  2 INCHES  3 YARDS  4 CENTIMETERS");
            return Console.ReadLine() switch
            {
                "1" => LengthUnit.FEET,
                "2" => LengthUnit.INCHES,
                "3" => LengthUnit.YARDS,
                "4" => LengthUnit.CENTIMETERS,
                _ => throw new ArgumentException("Invalid Length Unit")
            };
        }

        private static WeightUnit ReadWeightUnit()
        {
            Console.WriteLine("1 KILOGRAM  2 GRAM  3 POUND");
            return Console.ReadLine() switch
            {
                "1" => WeightUnit.KILOGRAM,
                "2" => WeightUnit.GRAM,
                "3" => WeightUnit.POUND,
                _ => throw new ArgumentException("Invalid Weight Unit")
            };
        }

        private static VolumeUnit ReadVolumeUnit()
        {
            Console.WriteLine("1 LITRE  2 MILLILITRE  3 GALLON");
            return Console.ReadLine() switch
            {
                "1" => VolumeUnit.LITRE,
                "2" => VolumeUnit.MILLILITRE,
                "3" => VolumeUnit.GALLON,
                _ => throw new ArgumentException("Invalid Volume Unit")
            };
        }

        private static TemperatureUnit ReadTemperatureUnit()
        {
            Console.WriteLine("1 CELSIUS  2 FAHRENHEIT  3 KELVIN");
            return Console.ReadLine() switch
            {
                "1" => TemperatureUnit.CELSIUS,
                "2" => TemperatureUnit.FAHRENHEIT,
                "3" => TemperatureUnit.KELVIN,
                _ => throw new ArgumentException("Invalid Temperature Unit")
            };
        }
    }
}