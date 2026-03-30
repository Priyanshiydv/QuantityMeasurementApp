namespace QuantityMeasurement.Service.Interfaces
{
    /// <summary>
    /// Interface for AES-256-GCM encryption service.
    /// Encrypts and decrypts sensitive fields.
    /// UC18
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>Encrypts plain text using AES-256-GCM.</summary>
        string Encrypt(string plainText);

        /// <summary>Decrypts AES-256-GCM cipher text.</summary>
        string Decrypt(string cipherText);
    }
}