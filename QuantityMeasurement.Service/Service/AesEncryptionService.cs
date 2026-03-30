using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QuantityMeasurement.Service.Interfaces;

namespace QuantityMeasurement.Service.Service
{
    /// <summary>
    /// AES-256-GCM encryption service for sensitive fields.
    /// Nonce is randomly generated per encryption and prepended to output.
    /// Format: [12-byte nonce][16-byte tag][N-byte ciphertext] → Base64
    /// UC18
    /// </summary>
    public class AesEncryptionService : IEncryptionService
    {
        private readonly byte[] _key;
        private readonly ILogger<AesEncryptionService> _logger;

        private const int NonceSize = 12;   // GCM standard nonce
        private const int TagSize   = 16;   // GCM authentication tag
        private const int KeySize   = 32;   // AES-256 = 32 bytes

        public AesEncryptionService(
            IConfiguration config,
            ILogger<AesEncryptionService> logger)
        {
            _logger = logger;

            string? keyBase64 = config["Encryption:Key"];

            if (string.IsNullOrWhiteSpace(keyBase64))
            {
                _logger.LogWarning(
                    "[AesEncryptionService] " +
                    "Encryption:Key not set. Using dev fallback key.");
                _key = DeriveKey("DevFallbackKey_ChangeInProduction!");
            }
            else
            {
                try
                {
                    _key = Convert.FromBase64String(keyBase64);
                    if (_key.Length != KeySize)
                        _key = DeriveKey(keyBase64);
                }
                catch
                {
                    _key = DeriveKey(keyBase64);
                }
            }
        }

        // ── Encrypt ───────────────────────────────────────────

        /// <summary>
        /// Encrypts plain text using AES-256-GCM.
        /// Each call generates a new random nonce for security.
        /// UC18
        /// </summary>
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] nonce      = RandomNumberGenerator.GetBytes(NonceSize);
            byte[] cipher     = new byte[plainBytes.Length];
            byte[] tag        = new byte[TagSize];

            using var aes = new AesGcm(_key, TagSize);
            aes.Encrypt(nonce, plainBytes, cipher, tag);

            // Pack: nonce + tag + cipher → Base64
            byte[] result = new byte[NonceSize + TagSize + cipher.Length];
            Buffer.BlockCopy(nonce,  0, result, 0,                    NonceSize);
            Buffer.BlockCopy(tag,    0, result, NonceSize,             TagSize);
            Buffer.BlockCopy(cipher, 0, result, NonceSize + TagSize,   cipher.Length);

            return Convert.ToBase64String(result);
        }

        // ── Decrypt ───────────────────────────────────────────

        /// <summary>
        /// Decrypts AES-256-GCM cipher text.
        /// Extracts nonce and tag from the packed Base64 string.
        /// UC18
        /// </summary>
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            byte[] full = Convert.FromBase64String(cipherText);

            if (full.Length < NonceSize + TagSize)
                return string.Empty;

            byte[] nonce  = new byte[NonceSize];
            byte[] tag    = new byte[TagSize];
            byte[] cipher = new byte[full.Length - NonceSize - TagSize];

            Buffer.BlockCopy(full, 0,                    nonce,  0, NonceSize);
            Buffer.BlockCopy(full, NonceSize,             tag,    0, TagSize);
            Buffer.BlockCopy(full, NonceSize + TagSize,   cipher, 0, cipher.Length);

            byte[] plain = new byte[cipher.Length];
            using var aes = new AesGcm(_key, TagSize);
            aes.Decrypt(nonce, cipher, tag, plain);

            return Encoding.UTF8.GetString(plain);
        }

        // ── Private Helper ────────────────────────────────────

        /// <summary>
        /// Derives a 32-byte AES key from any string using SHA-256.
        /// UC18
        /// </summary>
        private static byte[] DeriveKey(string secret)
        {
            using var sha = SHA256.Create();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(secret));
        }
    }
}