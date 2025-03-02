using Int20h2025.Auth.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Int20h2025.Auth.Services
{
    public class UserContextService(IMemoryCache memoryCache): IUserContextService
    {
        public Guid UserId { get; private set; }
        public string? UserData => memoryCache.Get(UserId)?.ToString();

        void IUserContextService.SetUser(Guid userId)
        {
            UserId = userId;
        }

        void IUserContextService.CacheData(string data)
        {
            memoryCache.Set(UserId, data);
        }
    }
}
