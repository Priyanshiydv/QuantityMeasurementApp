using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurement.Models.DTOs;
using QuantityMeasurement.Models.Entities;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Repository.Service;
using QuantityMeasurement.Service.Interfaces;

namespace QuantityMeasurement.Service.Service
{
    /// <summary>
    /// JWT Authentication service.
    ///
    /// REGISTER  BCrypt.HashPassword(password, 12)
    ///           Salt auto-embedded in hash → stored in Users table
    ///
    /// LOGIN     BCrypt.Verify(plain, storedHash) — timing-safe
    ///           On success: issues JWT (HMAC-SHA256) + refresh token
    ///
    /// REFRESH   Validates refresh token → rotates → issues new JWT
    ///
    /// GOOGLE    Validates Google ID token → issues JWT
    ///
    /// UC18
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository         _userRepo;
        private readonly IConfiguration          _config;
        private readonly ILogger<AuthService>    _logger;

        public AuthService(
            IUserRepository userRepo,
            IConfiguration config,
            ILogger<AuthService> logger)
        {
            _userRepo = userRepo;
            _config   = config;
            _logger   = logger;
        }

        // ── Register ──────────────────────────────────────────

        public async Task<AuthResponseDTO> RegisterAsync(
            RegisterRequestDTO req)
        {
            _logger.LogInformation(
                "[AuthService] Register: {U}", req.Username);

            if (await _userRepo.UsernameExistsAsync(req.Username))
                throw new InvalidOperationException(
                    "Username is already taken.");

            if (await _userRepo.EmailExistsAsync(req.Email))
                throw new InvalidOperationException(
                    "Email is already registered.");

            var user = new UserEntity
            {
                Username     = req.Username,
                Email        = req.Email,
                PasswordHash = UserRepository.HashPassword(req.Password),
                Role         = "User",
                CreatedAt    = DateTime.UtcNow,
                IsActive     = true
            };

            var saved = await _userRepo.CreateUserAsync(user);

            _logger.LogInformation(
                "[AuthService] Registered UserId={Id}", saved.Id);

            return await IssueTokensAsync(saved, "Registration successful.");
        }

        // ── Login ─────────────────────────────────────────────

        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO req)
        {
            _logger.LogInformation(
                "[AuthService] Login: {U}", req.Username);

            var user = await _userRepo.GetByUsernameAsync(req.Username);

            // Constant-time failure — never reveal if username exists
            if (user is null ||
                !UserRepository.VerifyPassword(req.Password, user.PasswordHash))
            {
                _logger.LogWarning(
                    "[AuthService] Failed login: {U}", req.Username);
                throw new UnauthorizedAccessException(
                    "Invalid username or password.");
            }

            if (!user.IsActive)
                throw new UnauthorizedAccessException(
                    "Account is disabled.");

            _logger.LogInformation(
                "[AuthService] Login success: UserId={Id}", user.Id);

            return await IssueTokensAsync(user, "Login successful.");
        }

        // ── Refresh Token ─────────────────────────────────────

        public async Task<AuthResponseDTO> RefreshTokenAsync(
            RefreshTokenRequestDTO req)
        {
            var principal = GetPrincipalFromExpireDTOken(req.AccessToken);

            string? idClaim = principal
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(idClaim, out int userId))
                throw new UnauthorizedAccessException("Invalid token.");

            var user = await _userRepo.GetByIdAsync(userId)
                ?? throw new UnauthorizedAccessException(
                    "User not found.");

            if (user.RefreshToken != req.RefreshToken ||
                user.RefreshTokenExpiry == null ||
                user.RefreshTokenExpiry <= DateTime.UtcNow)
                throw new UnauthorizedAccessException(
                    "Refresh token is invalid or expired.");

            _logger.LogInformation(
                "[AuthService] Token refreshed: UserId={Id}", userId);

            return await IssueTokensAsync(user, "Token refreshed.");
        }

        // ── Google Login ──────────────────────────────────────

        public async Task<AuthResponseDTO> GoogleLoginAsync(
            string googleToken)
        {
            _logger.LogInformation(
                "[AuthService] Google login attempt.");

            // Validate Google ID token via Google's tokeninfo endpoint
            using var http = new HttpClient();
            var response = await http.GetAsync(
                $"https://oauth2.googleapis.com/tokeninfo" +
                $"?id_token={googleToken}");

            if (!response.IsSuccessStatusCode)
                throw new UnauthorizedAccessException(
                    "Invalid Google token.");

            var json = await response.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var root = doc.RootElement;

            string googleId    = root.GetProperty("sub").GetString()!;
            string email       = root.GetProperty("email").GetString()!;
            string name        = root.TryGetProperty("name", out var n)
                ? n.GetString() ?? email : email;

            // Find existing user or create new one
            var user = await _userRepo.GetByGoogleIdAsync(googleId);

            if (user is null)
            {
                // Auto-register Google user
                user = new UserEntity
                {
                    Username    = name.Replace(" ", "_") + "_" +
                                  googleId[..6],
                    Email       = email,
                    PasswordHash = UserRepository.HashPassword(
                                  Guid.NewGuid().ToString()),
                    GoogleId    = googleId,
                    GoogleEmail = email,
                    Role        = "User",
                    CreatedAt   = DateTime.UtcNow,
                    IsActive    = true
                };
                user = await _userRepo.CreateUserAsync(user);

                _logger.LogInformation(
                    "[AuthService] Google user auto-registered: {E}",
                    email);
            }

            return await IssueTokensAsync(user, "Google login successful.");
        }

        // ── Private Helpers ───────────────────────────────────

        private async Task<AuthResponseDTO> IssueTokensAsync(
            UserEntity user, string message)
        {
            string accessToken  = GenerateJwt(user);
            string refreshToken = GenerateRefreshToken();
            DateTime expiry     = DateTime.UtcNow.AddDays(7);

            await _userRepo.UpdateRefreshTokenAsync(
                user.Id, refreshToken, expiry);

            return new AuthResponseDTO
            {
                AccessToken  = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt    = DateTime.UtcNow.AddMinutes(JwtExpireMinutes()),
                Username     = user.Username,
                Role         = user.Role,
                Message      = message
            };
        }

        private string GenerateJwt(UserEntity user)
        {
            string secret = _config["JwtSettings:Secret"]
                ?? throw new InvalidOperationException(
                    "JwtSettings:Secret is not configured.");

            string issuer   = _config["JwtSettings:Issuer"]
                              ?? "QuantityMeasurementAPI";
            string audience = _config["JwtSettings:Audience"]
                              ?? "QuantityMeasurementAPI";

            var key   = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(
                            key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,           user.Username),
                new Claim(ClaimTypes.Email,          user.Email),
                new Claim(ClaimTypes.Role,           user.Role),
                new Claim(JwtRegisteredClaimNames.Jti,
                          Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer:             issuer,
                audience:           audience,
                claims:             claims,
                expires:            DateTime.UtcNow.AddMinutes(
                                        JwtExpireMinutes()),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generates a 256-bit cryptographically random refresh token.
        /// UC18
        /// </summary>
        private static string GenerateRefreshToken()
            => Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(64));

        private ClaimsPrincipal GetPrincipalFromExpireDTOken(string token)
        {
            string secret = _config["JwtSettings:Secret"]!;

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey         = new SymmetricSecurityKey(
                                               Encoding.UTF8.GetBytes(secret)),
                ValidateIssuer           = false,
                ValidateAudience         = false,
                ValidateLifetime         = false // allow expired for refresh
            };

            return new JwtSecurityTokenHandler()
                .ValidateToken(token, validationParams, out _);
        }

        private int JwtExpireMinutes()
            => int.TryParse(
                _config["JwtSettings:ExpirationMinutes"], out int m)
                ? m : 60;
    }
}