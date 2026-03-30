using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuantityMeasurement.Models.Entities;
using QuantityMeasurement.Repository.Context;
using QuantityMeasurement.Repository.Interfaces;

namespace QuantityMeasurement.Repository.Service
{
    /// <summary>
    /// User persistence repository.
    /// Uses BCrypt for password hashing with auto-generated salt.
    /// Work factor 12 = 2^12 = 4096 Blowfish rounds (~250ms per hash).
    /// Salt is embedded inside the hash string — no separate column needed.
    /// UC18
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly QuantityMeasurementDbContext _db;
        private readonly ILogger<UserRepository> _logger;

        // BCrypt work factor — higher = slower = more secure
        private const int WorkFactor = 12;

        public UserRepository(
            QuantityMeasurementDbContext db,
            ILogger<UserRepository> logger)
        {
            _db     = db;
            _logger = logger;
        }

        // ── Create ────────────────────────────────────────────

        public async Task<UserEntity> CreateUserAsync(UserEntity user)
        {
            // Hash password if not already hashed
            if (!user.PasswordHash.StartsWith("$2"))
                user.PasswordHash = HashPassword(user.PasswordHash);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            _logger.LogInformation(
                "[UserRepository] User registered: Id={Id} Username={U}",
                user.Id, user.Username);

            return user;
        }

        // ── Read ──────────────────────────────────────────────

        public async Task<UserEntity?> GetByUsernameAsync(string username)
            => await _db.Users
                .FirstOrDefaultAsync(u =>
                    u.Username == username && u.IsActive);

        public async Task<UserEntity?> GetByIdAsync(int id)
            => await _db.Users.FindAsync(id);

        public async Task<UserEntity?> GetByGoogleIdAsync(string googleId)
            => await _db.Users
                .FirstOrDefaultAsync(u =>
                    u.GoogleId == googleId && u.IsActive);

        // ── Update ────────────────────────────────────────────

        public async Task UpdateRefreshTokenAsync(
            int userId, string refreshToken, DateTime expiry)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user is null) return;

            user.RefreshToken       = refreshToken;
            user.RefreshTokenExpiry = expiry;
            await _db.SaveChangesAsync();
        }

        // ── Existence Checks ──────────────────────────────────

        public async Task<bool> UsernameExistsAsync(string username)
            => await _db.Users.AnyAsync(u => u.Username == username);

        public async Task<bool> EmailExistsAsync(string email)
            => await _db.Users.AnyAsync(u => u.Email == email);

        // ── Static Password Helpers ───────────────────────────

        /// <summary>
        /// Hashes a plain-text password using BCrypt.
        /// Salt is auto-generated and embedded in the returned string.
        /// Format: $2a$12$[22-char-salt][31-char-hash]
        /// UC18
        /// </summary>
        public static string HashPassword(string plainText)
            => BCrypt.Net.BCrypt.HashPassword(plainText, WorkFactor);

        /// <summary>
        /// Verifies a plain-text password against a BCrypt hash.
        /// Timing-safe — always takes the same time whether match or not.
        /// UC18
        /// </summary>
        public static bool VerifyPassword(
            string plainText, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(plainText) ||
                string.IsNullOrWhiteSpace(storedHash))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(plainText, storedHash);
            }
            catch
            {
                return false;
            }
        }
    }
}