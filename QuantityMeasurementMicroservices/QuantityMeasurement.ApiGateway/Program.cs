using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── YARP Reverse Proxy ────────────────────────────
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(
        builder.Configuration.GetSection("ReverseProxy"));

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

// ── Controllers ───────────────────────────────────
builder.Services.AddControllers();

// ── Swagger ───────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Quantity Measurement - API Gateway",
        Version     = "v1",
        Description = "UC21: Microservices API Gateway — Routes to Auth Service (5001) and QMA Service (5002)"
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

var app = builder.Build();

// ── Swagger UI ────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json",
        "API Gateway v1");
    options.RoutePrefix = "swagger";
});

// ── Middleware ────────────────────────────────────
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ── Map Reverse Proxy ─────────────────────────────
app.MapReverseProxy();

Console.WriteLine("[ApiGateway] Running on port 5000 ✓");
Console.WriteLine("[ApiGateway] Swagger → http://localhost:5000/swagger");
Console.WriteLine("[ApiGateway] Auth    → http://localhost:5001");
Console.WriteLine("[ApiGateway] QMA     → http://localhost:5002");

app.Run();