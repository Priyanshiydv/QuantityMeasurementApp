using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // create service object
            QuantityMeasurementService service = new QuantityMeasurementService();

            // take user input 
            Console.Write("Enter first value in feet: ");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter second value in feet: ");
            double value2 = Convert.ToDouble(Console.ReadLine());

            // create Feet objects
            Feet feet1 = new Feet(value1);
            Feet feet2 = new Feet(value2);

            // compare equality
            bool result = service.AreEqual(feet1, feet2);

            // print result 
            Console.WriteLine("Equal: " + result);
        }
    }
}