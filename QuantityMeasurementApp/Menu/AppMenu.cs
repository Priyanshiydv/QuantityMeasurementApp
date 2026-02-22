using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Menu
{
    /// <summary>
    /// Handles application execution and menu.
    /// </summary>
    public class AppMenu
    {
        private static readonly QuantityMeasurementService service = 
            new QuantityMeasurementService();

        /// <summary>
        /// Starts the application.
        /// </summary>
        public static void Start()
        {
            string? choice;

            do
            {
                Console.WriteLine("---- Quantity Measurement App ----");
                Console.WriteLine("1. Compare Feet"); //UC1
                Console.WriteLine("2. Compare Inches"); //UC2
                Console.WriteLine("3. Compare Generic Quantity"); //UC3
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CompareFeet();
                        break;

                    case "2":
                        CompareInches();
                        break;

                    case "3":
                        CompareGenericQuantity();
                        break;

                    case "0":
                        Console.WriteLine("Exiting application...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }

                Console.WriteLine();

            } while (choice != "0");
        }

        /// <summary>
        /// UC1: Compare Feet values.
        /// </summary>
        private static void CompareFeet()
        {
            Console.Write("Enter first Feet value: ");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter second Feet value: ");
            double value2 = Convert.ToDouble(Console.ReadLine());

            Feet f1 = new Feet(value1);
            Feet f2 = new Feet(value2);

            bool result = service.AreEqual(f1, f2);

            Console.WriteLine("Feet Equality Result: " + result);
        }

        /// <summary>
        /// UC2: Compare Inches values.
        /// </summary>
        private static void CompareInches()
        {
            Console.Write("Enter first Inches value: ");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter second Inches value: ");
            double value2 = Convert.ToDouble(Console.ReadLine());

            Inches i1 = new Inches(value1);
            Inches i2 = new Inches(value2);

            bool result = service.AreEqual(i1, i2);
            Console.WriteLine("Inches Equality Result: " + result);
        }

        /// <summary>
        /// UC3: Compare Generic Quantity (Feet & Inch using DRY).
        /// </summary>
        private static void CompareGenericQuantity()
        {
            Console.Write("Enter first value: ");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Select unit for first value (1 = Feet, 2 = Inch): ");
            int unitChoice1 = Convert.ToInt32(Console.ReadLine());

            LengthUnit unit1 = unitChoice1 == 1 ? LengthUnit.Feet : LengthUnit.Inch;

            Console.Write("Enter second value: ");
            double value2 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Select unit for second value (1 = Feet, 2 = Inch): ");
            int unitChoice2 = Convert.ToInt32(Console.ReadLine());

            LengthUnit unit2 = unitChoice2 == 1 ? LengthUnit.Feet : LengthUnit.Inch;

            Quantity q1 = new Quantity(value1, unit1);
            Quantity q2 = new Quantity(value2, unit2);

            bool result = service.AreEqual(q1, q2);

            Console.WriteLine("Generic Quantity Equality Result: " + result);
        }
    }
}