using System.Security.Claims;

namespace Int20h2025.Auth.Interfaces
{
    public interface IAccessTokenService
    {
        string GenerateAccessToken(Dictionary<string, object> claims);
        bool TryValidateAccessToken(string token, out ClaimsPrincipal? claimsPrincipal);
    }
}
