using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace QuantityMeasurement.Repository.Utilities
{
    /// <summary>
    /// Loads and manages application configuration from appsettings.json.
    /// Provides centralized access to database configuration properties.
    /// Supports environment-specific configurations.
    /// UC16
    /// </summary>
    public class ApplicationConfig
    {
        // ─── Singleton Instance ───────────────────────────────

        private static ApplicationConfig? instance;
        private static readonly object lockObject = new object();

        // ─── Configuration Root ───────────────────────────────

        /// <summary>
        /// Root configuration object loaded from appsettings.json.
        /// </summary>
        private readonly IConfiguration configuration;

        // ─── Constants ────────────────────────────────────────

        /// <summary>
        /// Default repository type if not specified in config.
        /// </summary>
        private const string DefaultRepositoryType = "cache";

        /// <summary>
        /// Default connection timeout in seconds.
        /// </summary>
        private const int DefaultConnectionTimeout = 30;

        /// <summary>
        /// Default minimum pool size.
        /// </summary>
        private const int DefaultMinPoolSize = 2;

        /// <summary>
        /// Default maximum pool size.
        /// </summary>
        private const int DefaultMaxPoolSize = 10;

        // ─── Private Constructor (Singleton) ──────────────────

        /// <summary>
        /// Private constructor to enforce Singleton pattern.
        /// Loads configuration from appsettings.json file.
        /// </summary>
        private ApplicationConfig()
        {
            // Build configuration from appsettings.json
            // Searches in current directory and base directory
            configuration = new ConfigurationBuilder()
                .SetBasePath(GetConfigurationBasePath())
                .AddJsonFile("appsettings.json",
                             optional: false,
                             reloadOnChange: true)
                .Build();

            Console.WriteLine("[ApplicationConfig] Configuration loaded successfully.");
        }

        // ─── Singleton Access ─────────────────────────────────

        /// <summary>
        /// Returns the single instance of ApplicationConfig.
        /// Thread-safe Singleton implementation.
        /// </summary>
        public static ApplicationConfig GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new ApplicationConfig();
                    }
                }
            }
            return instance;
        }

        // ─── Repository Configuration ─────────────────────────

        /// <summary>
        /// Returns repository type from configuration.
        /// Checks environment variable first (set from Program.cs menu).
        /// Then falls back to appsettings.json value.
        /// Values: "cache" or "database"
        /// Default: "cache"
        /// </summary>
        public string GetRepositoryType()
        {
            // Step 1: Check environment variable first
            // This is set from Program.cs menu selection
            string? envValue = Environment.GetEnvironmentVariable(
                "REPOSITORY_TYPE");

            if (!string.IsNullOrWhiteSpace(envValue))
            {
                Console.WriteLine(
                    $"[ApplicationConfig] Repository type " +
                    $"from menu selection: {envValue}");
                return envValue;
            }

            // Step 2: Fall back to appsettings.json
            string? configValue = configuration[
                "DatabaseConfiguration:RepositoryType"];

            Console.WriteLine(
                $"[ApplicationConfig] Repository type " +
                $"from appsettings.json: " +
                $"{configValue ?? DefaultRepositoryType}");

            return configValue ?? DefaultRepositoryType;
        }

        /// <summary>
        /// Returns true if database repository should be used.
        /// </summary>
        public bool IsDatabaseRepository()
        {
            return GetRepositoryType()
                .Equals("database",
                         StringComparison.OrdinalIgnoreCase);
        }

        // ─── Connection String ────────────────────────────────

        /// <summary>
        /// Returns production database connection string.
        /// </summary>
        public string GetProductionConnectionString()
        {
            return configuration[
                "DatabaseConfiguration:ConnectionStrings:ProductionDatabase"]
                ?? throw new InvalidOperationException(
                    "Production connection string not found in appsettings.json");
        }

        /// <summary>
        /// Returns test database connection string.
        /// </summary>
        public string GetTestConnectionString()
        {
            return configuration[
                "DatabaseConfiguration:ConnectionStrings:TestDatabase"]
                ?? throw new InvalidOperationException(
                    "Test connection string not found in appsettings.json");
        }

        // ─── Connection Pool Configuration ────────────────────

        /// <summary>
        /// Returns minimum pool size from configuration.
        /// Default: 2
        /// </summary>
        public int GetMinPoolSize()
        {
            string? value = configuration[
                "DatabaseConfiguration:ConnectionPool:MinPoolSize"];

            return int.TryParse(value, out int result)
                ? result
                : DefaultMinPoolSize;
        }

        /// <summary>
        /// Returns maximum pool size from configuration.
        /// Default: 10
        /// </summary>
        public int GetMaxPoolSize()
        {
            string? value = configuration[
                "DatabaseConfiguration:ConnectionPool:MaxPoolSize"];

            return int.TryParse(value, out int result)
                ? result
                : DefaultMaxPoolSize;
        }

        /// <summary>
        /// Returns connection timeout in seconds.
        /// Default: 30
        /// </summary>
        public int GetConnectionTimeout()
        {
            string? value = configuration[
                "DatabaseConfiguration:ConnectionPool:ConnectionTimeout"];

            return int.TryParse(value, out int result)
                ? result
                : DefaultConnectionTimeout;
        }

        // ─── Application Configuration ────────────────────────

        /// <summary>
        /// Returns application environment.
        /// e.g. "Development", "Test", "Production"
        /// </summary>
        public string GetEnvironment()
        {
            return configuration["Application:Environment"]
                ?? "Development";
        }

        /// <summary>
        /// Returns application name from configuration.
        /// </summary>
        public string GetApplicationName()
        {
            return configuration["Application:Name"]
                ?? "QuantityMeasurementApp";
        }

        // ─── Helper Methods ───────────────────────────────────

        /// <summary>
        /// Gets the base path for configuration file.
        /// Checks current directory first then base directory.
        /// </summary>
        private string GetConfigurationBasePath()
        {
            // Check current directory first
            string currentDir = Directory.GetCurrentDirectory();

            if (File.Exists(Path.Combine(currentDir, "appsettings.json")))
                return currentDir;

            // Fall back to application base directory
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        // ─── ToString ─────────────────────────────────────────

        public override string ToString()
        {
            return $"ApplicationConfig [Environment: {GetEnvironment()}, " +
                   $"RepositoryType: {GetRepositoryType()}]";
        }
    }
}