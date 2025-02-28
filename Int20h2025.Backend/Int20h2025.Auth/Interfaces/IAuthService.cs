using Microsoft.AspNetCore.Http;

namespace Int20h2025.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<bool> ValidateRequestAsync(HttpContext context);
        void Logout();
        void SetupAuth(Guid userId);
    }
}
