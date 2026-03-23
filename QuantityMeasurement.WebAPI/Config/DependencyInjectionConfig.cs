using QuantityMeasurement.Repository.Context;
using QuantityMeasurement.Repository.Service;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Service.Interfaces;
using QuantityMeasurement.Service.Service;
using QuantityMeasurement.WebAPI.Filters;
namespace QuantityMeasurement.WebAPI.Config
{
    /// <summary>
    /// Centralized Dependency Injection configuration.
    /// Registers all services with ASP.NET Core DI Container.
    /// Replaces manual DI from UC15/UC16.
    /// UC17
    /// </summary>
    public static class DependencyInjectionConfig
    {
        /// <summary>
        /// Registers all application services.
        /// Called from Program.cs during startup.
        /// UC17
        /// </summary>
        public static void RegisterServices(
            IServiceCollection services)
        {
            // ─── Repository Layer ─────────────────────────────

            // Register EF Core Repository
            // Scoped = one instance per HTTP request
            // Replaces ADO.NET DatabaseRepository from UC16
            services.AddScoped
                <IQuantityMeasurementRepository,
                EFQuantityMeasurementRepository>();

            // ─── Service Layer ────────────────────────────────

            // Register Service Implementation
            // Scoped = one instance per HTTP request
            // Same business logic from UC15/UC16
            services.AddScoped
                <IQuantityMeasurementService,
                QuantityMeasurementServiceImpl>();

            Console.WriteLine(
                "[DependencyInjectionConfig] " +
                "Services registered ✓");

            // Register WebAPI Filters
            services.AddScoped<LoggingFilter>();
            services.AddScoped<ValidationFilter>();

            Console.WriteLine(
                "[DependencyInjectionConfig] " +
                "Filters registered ✓");
        }
    }
}