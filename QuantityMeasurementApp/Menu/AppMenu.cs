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
                Console.WriteLine("9. Volume Equality");       
                Console.WriteLine("10. Volume Conversion");    
                Console.WriteLine("11. Volume Addition"); 
                Console.WriteLine("12. Subtraction Operation");
                Console.WriteLine("13. Division Operation"); 
                Console.WriteLine("14. Cross Category Operations");
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
                    case "9":
                        VolumeEquality();
                        break;

                    case "10":
                        VolumeConversion();
                        break;

                    case "11":
                        VolumeAddition();
                        break;
                    case "12":
                        SubtractionDemo();
                        break;

                    case "13":
                        DivisionDemo();
                        break;
                    case "14":
                        CrossCategoryOperation();
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

        // ---------------- LENGTH ADDITION ----------------

        private static void LengthAddition()
        {
            Console.WriteLine("\nEnter First Length");
            var q1 = ReadLength();

            Console.WriteLine("\nEnter Second Length");
            var q2 = ReadLength();

            Console.WriteLine("\nChoose Addition Type:");
            Console.WriteLine("1. Use First Operand Unit (Implicit)");
            Console.WriteLine("2. Choose Target Unit (Explicit)");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                // Implicit addition (result in first operand unit)
                var result = q1.Add(q2);

                Console.WriteLine($"Addition Result: {result}");
            }
            else if (choice == "2")
            {
                Console.WriteLine("Select Result Unit:");
                LengthUnit resultUnit = ReadLengthUnit();

                var result = q1.Add(q2, resultUnit);

                Console.WriteLine($"Addition Result: {result}");
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
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

        // ---------------- WEIGHT ADDITION ----------------

        private static void WeightAddition()
        {
            Console.WriteLine("\nEnter First Weight");
            var w1 = ReadWeight();

            Console.WriteLine("\nEnter Second Weight");
            var w2 = ReadWeight();

            Console.WriteLine("\nChoose Addition Type:");
            Console.WriteLine("1. Use First Operand Unit (Implicit)");
            Console.WriteLine("2. Choose Target Unit (Explicit)");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                // Implicit addition
                var result = w1.Add(w2);

                Console.WriteLine($"Addition Result: {result}");
            }
            else if (choice == "2")
            {
                Console.WriteLine("Select Result Unit:");
                WeightUnit resultUnit = ReadWeightUnit();

                var result = w1.Add(w2, resultUnit);

                Console.WriteLine($"Addition Result: {result}");
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        // ---------------- CROSS CATEGORY ----------------

        private static void CrossCategoryEquality()
        {
            Console.WriteLine("\nSelect First Quantity Category:");
            Console.WriteLine("1. Length");
            Console.WriteLine("2. Weight");
            Console.WriteLine("3. Volume");
            Console.Write("Choice: ");

            string? choice1 = Console.ReadLine();

            object q1 = choice1 switch
            {
                "1" => ReadLength(),
                "2" => ReadWeight(),
                "3" => ReadVolume(),
                _ => throw new ArgumentException("Invalid category")
            };

            Console.WriteLine("\nSelect Second Quantity Category:");
            Console.WriteLine("1. Length");
            Console.WriteLine("2. Weight");
            Console.WriteLine("3. Volume");
            Console.Write("Choice: ");

            string? choice2 = Console.ReadLine();

            object q2 = choice2 switch
            {
                "1" => ReadLength(),
                "2" => ReadWeight(),
                "3" => ReadVolume(),
                _ => throw new ArgumentException("Invalid category")
            };

            // Different categories are incompatible
            Console.WriteLine("\nResult: False (Different Categories)");
        }


        // ---------------- GENERIC DEMO ----------------

        private static void GenericDemo()
        {
            Console.WriteLine("\n1. Length Generic Equality");
            Console.WriteLine("2. Weight Generic Equality");
            Console.WriteLine("3. Volume Generic Equality");
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
             else if (choice == "3")
            {
                var v1 = ReadVolume();
                var v2 = ReadVolume();
                DemonstrateEquality(v1, v2);
            }
            else
            {
                Console.WriteLine("Invalid choice.");
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

        // ---------------- VOLUME OPERATIONS ----------------

        private static void VolumeEquality()
        {
            Console.WriteLine("\nEnter First Volume");
            var v1 = ReadVolume();

            Console.WriteLine("\nEnter Second Volume");
            var v2 = ReadVolume();

            bool result = service.AreEqual(v1, v2);

            Console.WriteLine($"Result: {result}");
        }

        private static void VolumeConversion()
        {
            Console.WriteLine("\nEnter Volume to Convert");
            var volume = ReadVolume();

            Console.WriteLine("Convert To Unit:");
            VolumeUnit target = ReadVolumeUnit();

            var result = volume.ConvertTo(target);

            Console.WriteLine($"Converted: {result}");
        }

        // ---------------- VOLUME ADDITION ----------------

    private static void VolumeAddition()
    {
        Console.WriteLine("\nEnter First Volume");
        var v1 = ReadVolume();

        Console.WriteLine("\nEnter Second Volume");
        var v2 = ReadVolume();

        Console.WriteLine("\nChoose Addition Type:");
        Console.WriteLine("1. Use First Operand Unit (Implicit)");
        Console.WriteLine("2. Choose Target Unit (Explicit)");
        Console.Write("Choice: ");

        string? choice = Console.ReadLine();

        if (choice == "1")
        {
            var result = v1.Add(v2);

            Console.WriteLine($"Addition Result: {result}");
        }
        else if (choice == "2")
        {
            Console.WriteLine("Select Result Unit:");
            VolumeUnit resultUnit = ReadVolumeUnit();

            var result = v1.Add(v2, resultUnit);

            Console.WriteLine($"Addition Result: {result}");
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }

        private static QuantityGeneric<VolumeUnit> ReadVolume()
        {
            Console.Write("Enter Value: ");
            double value = Convert.ToDouble(Console.ReadLine());

            VolumeUnit unit = ReadVolumeUnit();

            return new QuantityGeneric<VolumeUnit>(value, unit);
        }

        private static VolumeUnit ReadVolumeUnit()
        {
            Console.WriteLine("Select Volume Unit:");
            Console.WriteLine("1. LITRE");
            Console.WriteLine("2. MILLILITRE");
            Console.WriteLine("3. GALLON");
            Console.Write("Choice: ");

            return Console.ReadLine() switch
            {
                "1" => VolumeUnit.LITRE,
                "2" => VolumeUnit.MILLILITRE,
                "3" => VolumeUnit.GALLON,
                _ => throw new ArgumentException("Invalid Volume Unit")
            };
        }

//==========================Subtraction Demonstration============================
        private static void SubtractionDemo()
        {
            Console.WriteLine("\nSelect Category:");
            Console.WriteLine("1. Length");
            Console.WriteLine("2. Weight");
            Console.WriteLine("3. Volume");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine();
            try
            {
                if (choice == "1")
                {
                    var q1 = ReadLength();
                    var q2 = ReadLength();

                    Console.WriteLine("1. Implicit Unit");
                    Console.WriteLine("2. Explicit Unit");

                    string? type = Console.ReadLine();

                    if (type == "1")
                    {
                        var result = q1.Subtract(q2);
                        Console.WriteLine($"Result: {result}");
                    }
                    else if (type == "2")
                    {
                        LengthUnit target = ReadLengthUnit();
                        var result = q1.Subtract(q2, target);
                        Console.WriteLine($"Result: {result}");
                    }
                }

                else if (choice == "2")
                {
                    var w1 = ReadWeight();
                    var w2 = ReadWeight();

                    Console.WriteLine("1. Implicit Unit");
                    Console.WriteLine("2. Explicit Unit");

                    string? type = Console.ReadLine();

                    if (type == "1")
                    {
                        var result = w1.Subtract(w2);
                        Console.WriteLine($"Result: {result}");
                    }
                    else if (type == "2")
                    {
                        WeightUnit target = ReadWeightUnit();
                        var result = w1.Subtract(w2, target);
                        Console.WriteLine($"Result: {result}");
                    }
                }

                else if (choice == "3")
                {
                    var v1 = ReadVolume();
                    var v2 = ReadVolume();

                    Console.WriteLine("1. Implicit Unit");
                    Console.WriteLine("2. Explicit Unit");

                    string? type = Console.ReadLine();

                    if (type == "1")
                    {
                        var result = v1.Subtract(v2);
                        Console.WriteLine($"Result: {result}");
                    }
                    else if (type == "2")
                    {
                        VolumeUnit target = ReadVolumeUnit();
                        var result = v1.Subtract(v2, target);
                        Console.WriteLine($"Result: {result}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

//==============================Division Demonstration===========================
        private static void DivisionDemo()
        {
            Console.WriteLine("\nSelect Category:");
            Console.WriteLine("1. Length");
            Console.WriteLine("2. Weight");
            Console.WriteLine("3. Volume");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine();

            try
            {
                if (choice == "1")
                {
                    var q1 = ReadLength();
                    var q2 = ReadLength();

                    double result = q1.Divide(q2);

                    Console.WriteLine($"Division Result: {result:F1}");
                }

                else if (choice == "2")
                {
                    var w1 = ReadWeight();
                    var w2 = ReadWeight();

                    double result = w1.Divide(w2);

                    Console.WriteLine($"Division Result: {result:F1}");
                }

                else if (choice == "3")
                {
                    var v1 = ReadVolume();
                    var v2 = ReadVolume();

                    double result = v1.Divide(v2);

                    Console.WriteLine($"Division Result: {result:F1}");
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArithmeticException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
//================Cross Category Operations===============================
        private static void CrossCategoryOperation()
        {
            Console.WriteLine("\n--- Cross Category Operations ---");
            Console.WriteLine("1. Cross Category Subtraction");
            Console.WriteLine("2. Cross Category Division");
            Console.Write("Choice: ");

            string? operationChoice = Console.ReadLine();

            try
            {
                var q1 = ReadAnyCategoryQuantity("First Quantity");
                var q2 = ReadAnyCategoryQuantity("Second Quantity");

                dynamic d1 = q1;
                dynamic d2 = q2;

                if (operationChoice == "1")
                {
                    var result = d1.Subtract(d2);
                    Console.WriteLine($"Result: {result}");
                }
                else if (operationChoice == "2")
                {
                    double result = d1.Divide(d2);
                    Console.WriteLine($"Division Result: {result:F2}");
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (ArithmeticException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private static object ReadAnyCategoryQuantity(string label)
        {
            Console.WriteLine($"\nSelect {label} Category:");
            Console.WriteLine("1. Length");
            Console.WriteLine("2. Weight");
            Console.WriteLine("3. Volume");
            Console.Write("Choice: ");

            string? choice = Console.ReadLine();

            return choice switch
            {
                "1" => ReadLength(),
                "2" => ReadWeight(),
                "3" => ReadVolume(),
                _ => throw new ArgumentException("Invalid category")
            };
        }
    }
}

