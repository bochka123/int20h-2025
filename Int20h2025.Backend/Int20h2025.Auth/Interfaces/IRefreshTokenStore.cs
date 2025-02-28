using Int20h2025.Auth.Models.Redis;

namespace Int20h2025.Auth.Interfaces
{
    public interface IRefreshTokenStore
    {
        Task SaveRefreshTokenAsync(string key, string value);
        Task<string?> GetRefreshTokenAsync(string key);
    }
}
