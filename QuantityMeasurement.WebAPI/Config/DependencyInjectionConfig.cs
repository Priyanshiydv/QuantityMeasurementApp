using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Repository.Service;
using QuantityMeasurement.Service.Interfaces;
using QuantityMeasurement.Service.Service;
using QuantityMeasurement.WebAPI.Filters;
using StackExchange.Redis;

namespace QuantityMeasurement.WebAPI.Config
{
    /// <summary>
    /// Centralized Dependency Injection configuration.
    /// UC17 services untouched — UC18 services added below.
    /// </summary>
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            // ─── UC17: Repository Layer ───────────────────────
            services.AddScoped
                <IQuantityMeasurementRepository,
                EFQuantityMeasurementRepository>();

            // ─── UC17: Service Layer ──────────────────────────
            services.AddScoped
                <IQuantityMeasurementService,
                QuantityMeasurementServiceImpl>();

            // ─── UC17: Filters ────────────────────────────────
            services.AddScoped<LoggingFilter>();
            services.AddScoped<ValidationFilter>();

            Console.WriteLine(
                "[DependencyInjectionConfig] UC17 services registered ✓");

            // ─── UC18: Redis or Memory Cache fallback ─────────
            string redisConn = configuration
                .GetConnectionString("Redis") ?? "localhost:6379";

            if (TryConnectRedis(redisConn))
            {
                services.AddStackExchangeRedisCache(opts =>
                {
                    opts.Configuration = redisConn;
                    opts.InstanceName  = "QM:";
                });
                Console.WriteLine(
                    $"[Startup] Redis connected: {redisConn} ✅");
            }
            else
            {
                services.AddMemoryCache();
                services.AddSingleton
                    <IDistributedCache,
                    MemoryDistributedCache>();
                Console.WriteLine(
                    "[Startup] Redis unavailable — " +
                    "using in-memory cache fallback. ");
            }

            // ─── UC18: User Repository ────────────────────────
            services.AddScoped
                <IUserRepository,
                UserRepository>();

            // ─── UC18: Auth Service ───────────────────────────
            services.AddScoped
                <IAuthService,
                AuthService>();

            // ─── UC18: AES Encryption Service ─────────────────
            services.AddScoped
                <IEncryptionService,
                AesEncryptionService>();

            // ─── UC18: Redis Service ──────────────────────────
            services.AddSingleton<RedisService>();

            // ─── UC18: Wire cache into CacheRepository ────────
            services.AddSingleton(sp =>
            {
                var cache = sp
                    .GetRequiredService<IDistributedCache>();
                var repo =
                    QuantityMeasurementCacheRepository
                    .GetInstance();
                repo.SetDistributedCache(cache);
                return repo;
            });

            Console.WriteLine(
                "[DependencyInjectionConfig] UC18 services registered ✓");
        }

        // ─── Private: Test Redis Connection ──────────────────

        private static bool TryConnectRedis(string connectionString)
        {
            try
            {
                using var redis =
                    ConnectionMultiplexer.Connect(
                        new ConfigurationOptions
                        {
                            EndPoints          = { connectionString },
                            ConnectTimeout     = 2000,
                            AbortOnConnectFail = false
                        });
                return redis.IsConnected;
            }
            catch
            {
                return false;
            }
        }
    }
}