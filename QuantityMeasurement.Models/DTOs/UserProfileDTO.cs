namespace QuantityMeasurement.Models.DTOs
{
    /// <summary>
    /// DTO for returning logged-in user's profile from JWT claims.
    /// UC18
    /// </summary>
    public class UserProfileDTO
    {
        public int    Id       { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email    { get; set; } = string.Empty;
        public string Role     { get; set; } = string.Empty;
    }
}