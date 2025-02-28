using Int20h2025.Auth.Interfaces;
using Int20h2025.Auth.Models.Settings;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Int20h2025.Auth.Services
{
    public class RedisRefreshTokenStore(IConnectionMultiplexer redis, AuthSettings authSettings) : IRefreshTokenStore
    {
        private readonly IDatabase _database = redis.GetDatabase();

        public async Task SaveRefreshTokenAsync(string key, string value)
        {
            key = GetKey(key);
            await _database.StringSetAsync(key, value, TimeSpan.FromDays(authSettings.Redis.DaysToExpire));
        }

        public async Task<string?> GetRefreshTokenAsync(string key)
        {
            key = GetKey(key);
            var value = await _database.StringGetAsync(key);
            if (!value.HasValue)
                return null;

            await _database.KeyDeleteAsync(key);

            return value.ToString();
        }

        private static string GetKey(string token) => $"refresh_token:{token}";
    }
}
