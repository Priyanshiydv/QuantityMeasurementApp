using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QuantityMeasurement.AuthService.Data;
using QuantityMeasurement.AuthService.Interfaces;
using QuantityMeasurement.AuthService.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers ───────────────────────────────────
builder.Services.AddControllers();

// ── Database ──────────────────────────────────────
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration
            .GetConnectionString("DefaultConnection")));

// ── JWT Authentication ────────────────────────────
var secret   = builder.Configuration["JwtSettings:Secret"]!;
var issuer   = builder.Configuration["JwtSettings:Issuer"]
               ?? "QuantityMeasurementAPI";
var audience = builder.Configuration["JwtSettings:Audience"]
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
    });

builder.Services.AddAuthorization();

// ── Swagger ───────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Auth Service API",
        Version     = "v1",
        Description = "UC21: Microservice — QMA Service"
    });

    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name         = "Authorization",
            Description  = "Enter: Bearer {token}",
            In           = ParameterLocation.Header,
            Type         = SecuritySchemeType.ApiKey,
            Scheme       = "Bearer",
            BearerFormat = "JWT"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
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
});
// ── CORS ──────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ── Register Services ─────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// ── Middleware ────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "Auth Service API v1");
});

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ── Ensure Database ───────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<AuthDbContext>();
    db.Database.EnsureCreated();
    Console.WriteLine("[AuthService] Database ready ✓");
}

Console.WriteLine("[AuthService] Running on port 5001 ✓");
app.Run();