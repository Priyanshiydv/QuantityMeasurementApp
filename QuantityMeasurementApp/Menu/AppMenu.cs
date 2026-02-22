using System;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Menu
{
    /// <summary>
    /// Handles application execution and menu.
    /// </summary>
    public class AppMenu
    {
        /// <summary>
        /// Starts the application.
        /// </summary>
        public static void Start()
        {
            string? choice;

            do
            {
                Console.WriteLine("---- Quantity Measurement App ----");
                Console.WriteLine("1. Compare Feet");
                Console.WriteLine("2. Compare Inches");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("Enter first Feet value: ");
                    double value1 = Convert.ToDouble(Console.ReadLine());

                    Console.Write("Enter second Feet value: ");
                    double value2 = Convert.ToDouble(Console.ReadLine());

                    bool result = QuantityMeasurementService.CompareFeet(value1, value2);

                    Console.WriteLine("Feet Equality Result: " + result);
                }
                else if (choice == "2")
                {
                    Console.Write("Enter first Inches value: ");
                    double value1 = Convert.ToDouble(Console.ReadLine());

                    Console.Write("Enter second Inches value: ");
                    double value2 = Convert.ToDouble(Console.ReadLine());

                    bool result = QuantityMeasurementService.CompareInches(value1, value2);

                    Console.WriteLine("Inches Equality Result: " + result);
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Exiting application...");
                }
                else
                {
                    Console.WriteLine("Invalid choice. Try again.");
                }

                Console.WriteLine();

            } while (choice != "3");
        }
    }
}