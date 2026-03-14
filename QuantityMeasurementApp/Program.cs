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
                {
                    // UC16 - Ask user which repository to use
                    Console.WriteLine(
                        "\n========================================");
                    Console.WriteLine(
                        "   SELECT REPOSITORY TYPE");
                    Console.WriteLine(
                        "========================================");
                    Console.WriteLine(
                        "1. Cache Repository    (In-Memory + JSON)");
                    Console.WriteLine(
                        "2. Database Repository (SQL Server)");
                    Console.WriteLine(
                        "========================================");
                    Console.Write("Choice: ");

                    string? repoChoice = Console.ReadLine();

                    if (repoChoice == "1")
                    {
                        // Set cache in appsettings.json at runtime
                        SetRepositoryType("cache");
                        Console.WriteLine(
                            "\n[Program] Cache Repository selected ✓");
                    }
                    else if (repoChoice == "2")
                    {
                        // Set database in appsettings.json at runtime
                        SetRepositoryType("database");
                        Console.WriteLine(
                            "\n[Program] Database Repository selected ✓");
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. " +
                            "Using config from appsettings.json.");
                    }

                    NTierMenu.Start();
                }

                else if (choice == "0")
                {
                    Console.WriteLine("Exiting Application...");
                    return;
                }
                else
                    Console.WriteLine("Invalid choice.");
            }
        }

        // ─── Helper Methods ───────────────────────────────────

        /// <summary>
        /// Sets repository type at runtime.
        /// Overrides appsettings.json value using
        /// ApplicationConfig singleton.
        /// Cache    → "cache"
        /// Database → "database"
        /// </summary>
        private static void SetRepositoryType(string repositoryType)
        {
            // Set environment variable so ApplicationConfig
            // picks it up without changing appsettings.json
            Environment.SetEnvironmentVariable(
                "REPOSITORY_TYPE", repositoryType);
        }
    }
}