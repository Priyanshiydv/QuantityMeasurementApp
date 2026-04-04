using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurement.AuthService.Data;
using QuantityMeasurement.AuthService.Models;
using QuantityMeasurement.AuthService.Interfaces;
namespace QuantityMeasurement.AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _db;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        private const int WorkFactor = 12;

        public AuthService(
            AuthDbContext db,
            IConfiguration config,
            ILogger<AuthService> logger)
        {
            _db     = db;
            _config = config;
            _logger = logger;
        }

        // ── Register ──────────────────────────────────
        public async Task<AuthResponse> RegisterAsync(
            RegisterRequest req)
        {
            _logger.LogInformation(
                "[AuthService] Register: {U}", req.Username);

            if (await _db.Users.AnyAsync(
                u => u.Username == req.Username))
                throw new InvalidOperationException(
                    "Username is already taken.");

            if (await _db.Users.AnyAsync(
                u => u.Email == req.Email))
                throw new InvalidOperationException(
                    "Email is already registered.");

            var user = new UserEntity
            {
                Username     = req.Username,
                Email        = req.Email,
                PasswordHash = BCrypt.Net.BCrypt
                    .HashPassword(req.Password, WorkFactor),
                Role         = "User",
                CreatedAt    = DateTime.UtcNow,
                IsActive     = true
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return await IssueTokensAsync(
                user, "Registration successful.");
        }

        // ── Login ─────────────────────────────────────
        public async Task<AuthResponse> LoginAsync(
            LoginRequest req)
        {
            _logger.LogInformation(
                "[AuthService] Login: {U}", req.Username);

            var user = await _db.Users
                .FirstOrDefaultAsync(u =>
                    u.Username == req.Username &&
                    u.IsActive);

            if (user is null ||
                !BCrypt.Net.BCrypt.Verify(
                    req.Password, user.PasswordHash))
                throw new UnauthorizedAccessException(
                    "Invalid username or password.");

            return await IssueTokensAsync(
                user, "Login successful.");
        }

        // ── Refresh Token ─────────────────────────────
        public async Task<AuthResponse> RefreshTokenAsync(
            RefreshTokenRequest req)
        {
            var principal = GetPrincipalFromExpiredToken(
                req.AccessToken);

            string? idClaim = principal
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(idClaim, out int userId))
                throw new UnauthorizedAccessException(
                    "Invalid token.");

            var user = await _db.Users.FindAsync(userId)
                ?? throw new UnauthorizedAccessException(
                    "User not found.");

            if (user.RefreshToken != req.RefreshToken ||
                user.RefreshTokenExpiry <= DateTime.UtcNow)
                throw new UnauthorizedAccessException(
                    "Refresh token is invalid or expired.");

            return await IssueTokensAsync(
                user, "Token refreshed.");
        }

        // ── Google Login ──────────────────────────────
        public async Task<AuthResponse> GoogleLoginAsync(
            string accessToken)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Add(
                "Authorization", $"Bearer {accessToken}");

            var response = await http.GetAsync(
                "https://www.googleapis.com/oauth2/v3/userinfo");

            if (!response.IsSuccessStatusCode)
                throw new UnauthorizedAccessException(
                    "Invalid Google token.");

            var json = await response.Content
                .ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument
                .Parse(json);
            var root = doc.RootElement;

            string googleId = root
                .GetProperty("sub").GetString()!;
            string email    = root
                .GetProperty("email").GetString()!;
            string name     = root.TryGetProperty(
                "name", out var n)
                ? n.GetString() ?? email : email;

            var user = await _db.Users
                .FirstOrDefaultAsync(u =>
                    u.GoogleId == googleId);

            if (user is null)
            {
                user = new UserEntity
                {
                    Username     = name.Replace(" ", "_")
                                   + "_" + googleId[..6],
                    Email        = email,
                    PasswordHash = BCrypt.Net.BCrypt
                        .HashPassword(
                            Guid.NewGuid().ToString(),
                            WorkFactor),
                    GoogleId     = googleId,
                    GoogleEmail  = email,
                    Role         = "User",
                    CreatedAt    = DateTime.UtcNow,
                    IsActive     = true
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }

            return await IssueTokensAsync(
                user, "Google login successful.");
        }

        // ── Private Helpers ───────────────────────────
        private async Task<AuthResponse> IssueTokensAsync(
            UserEntity user, string message)
        {
            string accessToken  = GenerateJwt(user);
            string refreshToken = Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(64));
            DateTime expiry = DateTime.UtcNow.AddDays(7);

            user.RefreshToken       = refreshToken;
            user.RefreshTokenExpiry = expiry;
            await _db.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken  = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt    = DateTime.UtcNow
                    .AddMinutes(JwtExpireMinutes()),
                Username     = user.Username,
                Role         = user.Role,
                Message      = message
            };
        }

        private string GenerateJwt(UserEntity user)
        {
            string secret = _config["JwtSettings:Secret"]!;
            string issuer = _config["JwtSettings:Issuer"]
                            ?? "QuantityMeasurementAPI";
            string audience = _config["JwtSettings:Audience"]
                              ?? "QuantityMeasurementAPI";

            var key   = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,
                    user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer:             issuer,
                audience:           audience,
                claims:             claims,
                expires:            DateTime.UtcNow
                    .AddMinutes(JwtExpireMinutes()),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(
            string token)
        {
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _config["JwtSettings:Secret"]!)),
                ValidateIssuer   = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            return new JwtSecurityTokenHandler()
                .ValidateToken(token, validationParams, out _);
        }

        private int JwtExpireMinutes()
            => int.TryParse(
                _config["JwtSettings:ExpirationMinutes"],
                out int m) ? m : 60;
    }
}