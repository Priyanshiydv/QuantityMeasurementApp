using QuantityMeasurement.Models.DTOs;

namespace QuantityMeasurement.Service.Interfaces
{
    /// <summary>
    /// Interface for JWT authentication service.
    /// Handles Register, Login, Refresh, and Google OAuth.
    /// UC18
    /// </summary>
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO request);
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request);
        Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO request);
        Task<AuthResponseDTO> GoogleLoginAsync(string googleToken);
    }
}