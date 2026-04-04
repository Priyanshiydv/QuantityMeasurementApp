using QuantityMeasurement.AuthService.Models;

namespace QuantityMeasurement.AuthService.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<AuthResponse> GoogleLoginAsync(string accessToken);
    }
}