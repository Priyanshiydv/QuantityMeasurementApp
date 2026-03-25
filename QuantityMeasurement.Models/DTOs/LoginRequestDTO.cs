using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurement.Models.DTOs
{
    /// <summary>
    /// DTO for user login request.
    /// UC18
    /// </summary>
    public class LoginRequestDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}