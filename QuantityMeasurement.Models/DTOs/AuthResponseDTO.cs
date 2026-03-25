namespace QuantityMeasurement.Models.DTOs
{
    /// <summary>
    /// DTO returned after successful login, register, or token refresh.
    /// UC18
    /// </summary>
    public class AuthResponseDTO
    {
        public string AccessToken  { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt  { get; set; }
        public string Username     { get; set; } = string.Empty;
        public string Role         { get; set; } = string.Empty;
        public string Message      { get; set; } = string.Empty;
    }
}