using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurement.Models.DTOs
{
    /// <summary>
    /// DTO for Google OAuth login request.
    /// UC18
    /// </summary>
    public class GoogleLoginRequestDTO
    {
        [Required]
        public string IdToken { get; set; } = string.Empty;
    }
}