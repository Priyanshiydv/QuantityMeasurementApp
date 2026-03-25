using QuantityMeasurement.Models.Entities;

namespace QuantityMeasurement.Repository.Interfaces
{
    /// <summary>
    /// Repository interface for User data access.
    /// UC18
    /// </summary>
    public interface IUserRepository
    {
        Task<UserEntity> CreateUserAsync(UserEntity user);
        Task<UserEntity?> GetByUsernameAsync(string username);
        Task<UserEntity?> GetByIdAsync(int id);
        Task<UserEntity?> GetByGoogleIdAsync(string googleId);
        Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiry);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
    }
}