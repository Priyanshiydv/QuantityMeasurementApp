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
                Console.WriteLine("\n===== QUANTITY MEASUREMENT APP =====");
                Console.WriteLine("1. Length Equality");
                Console.WriteLine("2. Length Conversion");
                Console.WriteLine("3. Length Addition");
                Console.WriteLine("4. Weight Equality");
                Console.WriteLine("5. Weight Conversion");
                Console.WriteLine("6. Weight Addition");
                Console.WriteLine("7. Cross Category Equality");
                Console.WriteLine("8. Generic Demonstration");
                Console.WriteLine("0. Exit");
                Console.Write("Select Option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        LengthEquality();
                        break;

                    case "2":
                        LengthConversion();
                        break;

                    case "3":
                        LengthAddition();
                        break;

                    case "4":
                        WeightEquality();
                        break;

                    case "5":
                        WeightConversion();
                        break;

                    case "6":
                        WeightAddition();
                        break;

                    case "7":
                        CrossCategoryEquality();
                        break;

                    case "8":
                        GenericDemo();
                        break;

                    case "0":
                        Console.WriteLine("Exiting Application...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        // ---------------- LENGTH OPERATIONS ----------------

        private static void LengthEquality()
        {
            Console.WriteLine("\nEnter First Length");
            var q1 = ReadLength();

            Console.WriteLine("\nEnter Second Length");
            var q2 = ReadLength();

            bool result = service.AreEqual(q1, q2);

            Console.WriteLine($"Result: {result}");
        }

        private static void LengthConversion()
        {
            Console.WriteLine("\nEnter Length to Convert");
            var quantity = ReadLength();

            Console.WriteLine("Convert To Unit:");
            LengthUnit target = ReadLengthUnit();

            var result = quantity.ConvertTo(target);

            Console.WriteLine($"Converted: {result}");
        }

        private static void LengthAddition()
        {
            Console.WriteLine("\nEnter First Length");
            var q1 = ReadLength();

            Console.WriteLine("\nEnter Second Length");
            var q2 = ReadLength();

            Console.WriteLine("Result Unit:");
            LengthUnit resultUnit = ReadLengthUnit();

            var result = q1.Add(q2, resultUnit);

            Console.WriteLine($"Addition Result: {result}");
        }

        // ---------------- WEIGHT OPERATIONS ----------------

        private static void WeightEquality()
        {
            Console.WriteLine("\nEnter First Weight");
            var w1 = ReadWeight();

            Console.WriteLine("\nEnter Second Weight");
            var w2 = ReadWeight();

            bool result = service.AreEqual(w1, w2);

            Console.WriteLine($"Result: {result}");
        }

        private static void WeightConversion()
        {
            Console.WriteLine("\nEnter Weight to Convert");
            var weight = ReadWeight();

            Console.WriteLine("Convert To Unit:");
            WeightUnit target = ReadWeightUnit();

            var result = weight.ConvertTo(target);

            Console.WriteLine($"Converted: {result}");
        }

        private static void WeightAddition()
        {
            Console.WriteLine("\nEnter First Weight");
            var w1 = ReadWeight();

            Console.WriteLine("\nEnter Second Weight");
            var w2 = ReadWeight();

            Console.WriteLine("Result Unit:");
            WeightUnit resultUnit = ReadWeightUnit();

            var result = w1.Add(w2, resultUnit);

            Console.WriteLine($"Addition Result: {result}");
        }

        // ---------------- CROSS CATEGORY ----------------

        private static void CrossCategoryEquality()
        {
            Console.WriteLine("\nEnter Length");
            var length = ReadLength();

            Console.WriteLine("\nEnter Weight");
            var weight = ReadWeight();

            bool result = length.Equals(weight);

            Console.WriteLine($"Result: {result}");
        }

        // ---------------- GENERIC DEMO ----------------

        private static void GenericDemo()
        {
            Console.WriteLine("\n1. Length Generic Equality");
            Console.WriteLine("2. Weight Generic Equality");
            Console.Write("Choose: ");

            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                var q1 = ReadLength();
                var q2 = ReadLength();
                DemonstrateEquality(q1, q2);
            }
            else if (choice == "2")
            {
                var w1 = ReadWeight();
                var w2 = ReadWeight();
                DemonstrateEquality(w1, w2);
            }
        }

        // ---------------- GENERIC METHOD ----------------

        private static void DemonstrateEquality<T>(QuantityGeneric<T> q1, QuantityGeneric<T> q2)
            where T : IMeasurable
        {
            Console.WriteLine("\n---- Generic Equality Demonstration ----");
            Console.WriteLine($"Quantity 1: {q1}");
            Console.WriteLine($"Quantity 2: {q2}");
            Console.WriteLine($"Are Equal? {q1.Equals(q2)}");
        }

        // ---------------- INPUT HELPERS ----------------

        private static QuantityGeneric<LengthUnit> ReadLength()
        {
            Console.Write("Enter Value: ");
            double value = Convert.ToDouble(Console.ReadLine());

            LengthUnit unit = ReadLengthUnit();

            return new QuantityGeneric<LengthUnit>(value, unit);
        }

        private static LengthUnit ReadLengthUnit()
        {
            Console.WriteLine("Select Length Unit:");
            Console.WriteLine("1. FEET");
            Console.WriteLine("2. INCHES");
            Console.WriteLine("3. YARDS");
            Console.WriteLine("4. CENTIMETERS");
            Console.Write("Choice: ");

            return Console.ReadLine() switch
            {
                "1" => LengthUnit.FEET,
                "2" => LengthUnit.INCHES,
                "3" => LengthUnit.YARDS,
                "4" => LengthUnit.CENTIMETERS,
                _ => throw new ArgumentException("Invalid Length Unit")
            };
        }

        private static QuantityGeneric<WeightUnit> ReadWeight()
        {
            Console.Write("Enter Value: ");
            double value = Convert.ToDouble(Console.ReadLine());

            WeightUnit unit = ReadWeightUnit();

            return new QuantityGeneric<WeightUnit>(value, unit);
        }

        private static WeightUnit ReadWeightUnit()
        {
            Console.WriteLine("Select Weight Unit:");
            Console.WriteLine("1. KILOGRAM");
            Console.WriteLine("2. GRAM");
            Console.WriteLine("3. POUND");
            Console.Write("Choice: ");

            return Console.ReadLine() switch
            {
                "1" => WeightUnit.KILOGRAM,
                "2" => WeightUnit.GRAM,
                "3" => WeightUnit.POUND,
                _ => throw new ArgumentException("Invalid Weight Unit")
            };
        }
    }
}