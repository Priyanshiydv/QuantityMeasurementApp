using NLog;
using NLog.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QuantityMeasurement.Repository.Context;
using QuantityMeasurement.WebAPI.Config;
using QuantityMeasurement.WebAPI.Exceptions;
using QuantityMeasurement.WebAPI.Filters;

namespace QuantityMeasurement.WebAPI
{
    /// <summary>
    /// Main entry point for Quantity Measurement WebAPI.
    /// Configures all services and middleware pipeline.
    /// UC17
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Setup NLog for dependency injection
            var logger = LogManager
                .Setup()
                .LoadConfigurationFromFile(Path.Combine("nlog.config"))
                .GetCurrentClassLogger();

            try
            {
                logger.Info("[Program] Starting Quantity Measurement WebAPI...");
            
                    // ─── Create Builder ───────────────────────────────
                    var builder = WebApplication.CreateBuilder(args);
                    // Add NLog to ASP.NET Core
                    builder.Logging.ClearProviders();
                    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    builder.Host.UseNLog();

                    // ─── Add Controllers ──────────────────────────────
                    builder.Services.AddControllers(options =>
                    {
                        // Add filters globally to all controllers
                        options.Filters.AddService<LoggingFilter>();
                        options.Filters.AddService<ValidationFilter>();
                    })
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.NullValueHandling =
                            Newtonsoft.Json.NullValueHandling.Ignore;
                        options.SerializerSettings.ContractResolver =
                            new Newtonsoft.Json.Serialization
                                .CamelCasePropertyNamesContractResolver();
                    });

                    // ─── Add Database Context ─────────────────────────
                    bool useInMemory = builder.Configuration
                        .GetValue<bool>(
                            "DatabaseConfiguration:UseInMemoryDatabase");

                    if (useInMemory)
                    {
                        string dbName = builder.Configuration
                            .GetValue<string>(
                                "DatabaseConfiguration:InMemoryDatabaseName")
                            ?? "QuantityMeasurementInMemoryDB";

                        builder.Services
                            .AddDbContext<QuantityMeasurementDbContext>(
                                options => options
                                    .UseInMemoryDatabase(dbName));

                        Console.WriteLine(
                            $"[Program] Using InMemory Database: {dbName}");
                    }
                    else
                    {
                        string? connStr = builder.Configuration
                            .GetConnectionString("DefaultConnection");

                        if (connStr == null)
                            throw new InvalidOperationException(
                                "Connection string not found!");

                        builder.Services
                            .AddDbContext<QuantityMeasurementDbContext>(
                                options => options
                                    .UseSqlServer(connStr));

                        Console.WriteLine(
                            "[Program] Using SQL Server Database");
                    }

                    // ─── Add Swagger ──────────────────────────────────
                    builder.Services.AddEndpointsApiExplorer();
                    builder.Services.AddSwaggerGen(options =>
                    {
                        options.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Title       = "Quantity Measurement API",
                            Version     = "v1",
                            Description = "REST API for Quantity Measurement",
                            Contact     = new OpenApiContact
                            {
                                Name = "Priyanshi Yadav"
                            }
                        });
                        options.EnableAnnotations();
                    });

                    // ─── Add CORS ─────────────────────────────────────
                    builder.Services.AddCors(options =>
                    {
                        options.AddPolicy("AllowAll", policy =>
                        {
                            policy
                                .AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                        });
                    });

                    // ─── Add Dependency Injection ─────────────────────
                    DependencyInjectionConfig.RegisterServices(
                        builder.Services);

                    // ─── Build Application ────────────────────────────
                    var app = builder.Build();

                    // ─── Configure Middleware Pipeline ────────────────
                    if (app.Environment.IsDevelopment())
                    {
                        app.UseSwagger();
                        app.UseSwaggerUI(options =>
                        {
                            options.SwaggerEndpoint(
                                "/swagger/v1/swagger.json",
                                "Quantity Measurement API v1");
                            options.RoutePrefix = "swagger";
                        });

                        Console.WriteLine(
                            "[Program] Swagger UI at: " +
                            "http://localhost:5092/swagger");
                    }

                    // Global exception handling middleware
                    app.UseMiddleware<GlobalExceptionMiddleware>();

                    // Enable CORS
                    app.UseCors("AllowAll");

                    // Enable HTTPS redirection
                    app.UseHttpsRedirection();

                    // Enable routing
                    app.UseRouting();

                    // Enable authorization
                    app.UseAuthorization();

                    // Map controllers
                    app.MapControllers();

                    // Serve static files
                    app.UseStaticFiles();

                    // ─── Initialize Database ──────────────────────────
                    using (var scope = app.Services.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider
                            .GetRequiredService<QuantityMeasurementDbContext>();

                        dbContext.Database.EnsureCreated();

                        Console.WriteLine(
                            "[Program] Database ready ✓");
                    }

                    Console.WriteLine(
                        "[Program] WebAPI started ✓");

                    app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex,
                    "[Program] Stopped due to exception!");
                throw;
            }
            finally
            {
                // Flush and stop NLog
                LogManager.Shutdown();
            }
        }
    }
}