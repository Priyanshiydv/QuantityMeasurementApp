using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurement.Models.Entities
{
    /// <summary>
    /// User entity mapped to [Users] table in SQL Server.
    /// PasswordHash stores BCrypt output — never plain text.
    /// UC18
    /// </summary>
    [Table("Users")]
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// BCrypt hash: $2a$12$[22-char-salt][31-char-hash]
        /// Salt is embedded — no separate salt column needed.
        /// Work factor 12 = 4096 rounds (~250ms per hash).
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        [MaxLength(512)]
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiry { get; set; }

        [MaxLength(200)]
        public string? GoogleId { get; set; }

        [MaxLength(200)]
        public string? GoogleEmail { get; set; }
    }
}