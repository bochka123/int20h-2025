using Int20h2025.Auth.Models.Redis;

namespace Int20h2025.Auth.Interfaces
{
    public interface IRefreshTokenService
    {
        RefreshTokenResponse GenerateRefreshToken(Guid userId, string ip);
        Task<RefreshTokenResponse?> GetValidatedRefreshTokenAsync(RefreshTokenValidateModel model);
    }
}
