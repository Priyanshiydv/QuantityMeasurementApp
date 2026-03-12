using QuantityMeasurementApp.Menu;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n========================================");
                Console.WriteLine("   QUANTITY MEASUREMENT SYSTEM");
                Console.WriteLine("========================================");
                Console.WriteLine("1. AppMenu  (Old App)");  //UC1-UC14
                Console.WriteLine("2. NTierMenu      (N-Tier App)"); //UC15
                Console.WriteLine("0. Exit");
                Console.WriteLine("========================================");
                Console.Write("Choice: ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                    AppMenu.Start();
                else if (choice == "2")
                    NTierMenu.Start();

                else if (choice == "0")
                {
                    Console.WriteLine("Exiting Application...");
                    return;
                }
                else
                    Console.WriteLine("Invalid choice.");
            }
        }
    }
}