using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurement.Models.DTOs
{
    /// <summary>
    /// DTO for refresh token request.
    /// UC18
    /// </summary>
    public class RefreshTokenRequestDTO
    {
        [Required]
        public string AccessToken  { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}