using NLog;
using NLog.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QuantityMeasurement.Repository.Context;
using QuantityMeasurement.WebAPI.Config;
using QuantityMeasurement.WebAPI.Exceptions;
using QuantityMeasurement.WebAPI.Filters;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace QuantityMeasurement.WebAPI
{
    /// <summary>
    /// Main entry point for Quantity Measurement WebAPI.
    /// UC17: REST API, EF Core, SQL Server, NLog, Swagger
    /// UC18: JWT Auth, BCrypt, AES-256-GCM, Redis, Google OAuth
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager
                .Setup()
                .LoadConfigurationFromFile("nlog.config")
                .GetCurrentClassLogger();

            try
            {
                logger.Info("[Program] Starting Quantity Measurement WebAPI...");

                var builder = WebApplication.CreateBuilder(args);

                // ─── NLog ─────────────────────────────────────
                builder.Logging.ClearProviders();
                builder.Logging.SetMinimumLevel(
                    Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.Host.UseNLog();

                // ─── Controllers ──────────────────────────────
                builder.Services.AddControllers(options =>
                {
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

                // ─── Database ─────────────────────────────────
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
                            options => options.UseInMemoryDatabase(dbName));

                    Console.WriteLine(
                        $"[Program] Using InMemory Database: {dbName}");
                }
                else
                {
                    // Check for PostgreSQL first (Render deployment)
                    string? pgConnStr = builder.Configuration
                        .GetConnectionString("PostgresConnection");

                    string? sqlConnStr = builder.Configuration
                        .GetConnectionString("DefaultConnection");

                    if (!string.IsNullOrEmpty(pgConnStr))
                    {
                        builder.Services
                            .AddDbContext<QuantityMeasurementDbContext>(
                                options => options.UseNpgsql(pgConnStr));
                        Console.WriteLine(
                            "[Program] Using PostgreSQL Database");
                    }
                    else if (!string.IsNullOrEmpty(sqlConnStr))
                    {
                        builder.Services
                            .AddDbContext<QuantityMeasurementDbContext>(
                                options => options.UseSqlServer(sqlConnStr));
                        Console.WriteLine(
                            "[Program] Using SQL Server Database");
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "No database connection string found!");
                    }
                }

                // ─── UC18: JWT Authentication ─────────────────
                var config = builder.Configuration;
                string secret = config["JwtSettings:Secret"]
                    ?? throw new InvalidOperationException(
                        "JwtSettings:Secret is not configured.");
                string issuer   = config["JwtSettings:Issuer"]
                                  ?? "QuantityMeasurementAPI";
                string audience = config["JwtSettings:Audience"]
                                  ?? "QuantityMeasurementAPI";

                builder.Services
                    .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme =
                            JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme =
                            JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters =
                            new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey =
                                    new SymmetricSecurityKey(
                                        Encoding.UTF8.GetBytes(secret)),
                                ValidateIssuer   = true,
                                ValidIssuer      = issuer,
                                ValidateAudience = true,
                                ValidAudience    = audience,
                                ValidateLifetime = true,
                                ClockSkew        = TimeSpan.FromMinutes(1)
                            };

                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = ctx =>
                            {
                                if (ctx.Exception is
                                    SecurityTokenExpiredException)
                                    ctx.Response.Headers.Append(
                                        "Token-Expired", "true");
                                return Task.CompletedTask;
                            }
                        };
                    });

                builder.Services.AddAuthorization();

                // ─── UC18: Google OAuth ───────────────────────
                builder.Services
                    .AddAuthentication()
                    .AddGoogle(options =>
                    {
                        options.ClientId =
                            config["GoogleOAuth:ClientId"] ?? "";
                        options.ClientSecret =
                            config["GoogleOAuth:ClientSecret"] ?? "";
                    });

                // ─── Swagger ──────────────────────────────────
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title       = "Quantity Measurement API",
                        Version     = "v1",
                        Description =
                            "UC17: REST API | UC18: JWT + BCrypt + AES + Redis + Google OAuth",
                        Contact = new OpenApiContact
                        {
                            Name = "Priyanshi Yadav"
                        }
                    });

                    // UC18: Add Bearer token support in Swagger UI
                    var securityScheme = new OpenApiSecurityScheme
                    {
                        Name        = "Authorization",
                        Description = "Enter: Bearer {your_jwt_token}",
                        In          = ParameterLocation.Header,
                        Type        = SecuritySchemeType.ApiKey,
                        Scheme      = "Bearer",
                        BearerFormat = "JWT"
                    };
                    options.AddSecurityDefinition("Bearer", securityScheme);
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id   = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });

                    options.EnableAnnotations();
                });

                // ─── CORS ─────────────────────────────────────
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", policy =>
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader());
                });

                // ─── Dependency Injection ─────────────────────
                DependencyInjectionConfig.RegisterServices(
                    builder.Services, builder.Configuration);

                // ─── Build App ────────────────────────────────
                var app = builder.Build();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                        "/swagger/v1/swagger.json",
                        "Quantity Measurement API v1");
                    options.RoutePrefix = "swagger";
                });

                Console.WriteLine(
                    "[Program] Swagger UI enabled ✓");

                app.UseMiddleware<GlobalExceptionMiddleware>();
                app.UseCors("AllowAll");
                app.UseHttpsRedirection();
                app.UseRouting();

                // UC18: Authentication MUST come before Authorization
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();
                app.UseStaticFiles();

                // ─── Initialize Database ──────────────────────
                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider
                        .GetRequiredService<QuantityMeasurementDbContext>();

                    dbContext.Database.EnsureCreated();
                    Console.WriteLine("[Program] Database ready ✓");
                }

                Console.WriteLine("[Program] WebAPI started ✓");
                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "[Program] Stopped due to exception!");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}