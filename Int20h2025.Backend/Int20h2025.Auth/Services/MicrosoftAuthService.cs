using Int20h2025.Auth.Context;
using Int20h2025.Auth.Entities;
using Int20h2025.Auth.Interfaces;
using Int20h2025.Auth.Models.DTO;
using Int20h2025.Auth.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Int20h2025.Auth.Services
{
    public class MicrosoftAuthService(AuthSettings authSettings, IUserContextService userContextService, IAuthContext context, IAuthService authService) : IMicrosoftAuthService
    {
        public async Task SignInAsync(MicrosoftSignModel model)
        {
            if (string.IsNullOrEmpty(model.AccessToken))
                throw new ArgumentException("Access token is required");

            var userClaims = await ValidateAndParseToken(model.AccessToken)
                ?? throw new UnauthorizedAccessException("Invalid Microsoft access token.");

            var email = userClaims.Identity?.Name;

            if (string.IsNullOrEmpty(email))
                throw new UnauthorizedAccessException("Email is missing in the token");

            var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
            }

            authService.SetupAuth(user.Id);
            userContextService.CacheData(model.AccessToken);
        }

        private async Task<ClaimsPrincipal?> ValidateAndParseToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var publikKeys = await GetPublicKeysAsync();
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKeys = publikKeys.Keys,
                ValidIssuer = $"https://sts.windows.net/{authSettings.AzureAd.TenantId}/",
                ValidateIssuer = true,
                ValidAudience = authSettings.AzureAd.Audience,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            try
            {
                var principal = tokenHandler.ValidateToken(accessToken, validationParameters, out _);
                return principal;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
        }

        private async Task<JsonWebKeySet> GetPublicKeysAsync()
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync($"https://login.microsoftonline.com/{authSettings.AzureAd.TenantId}/discovery/v2.0/keys");
            return new JsonWebKeySet(response);
        }
    }
}
