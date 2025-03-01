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
    public class MicrosoftAuthService(AuthSettings authSettings, HttpClient _httpClient, IAuthContext context, IAuthService authService) : IMicrosoftAuthService
    {
        public async Task SignInAsync(MicrosoftSignModel model)
        {
            if (string.IsNullOrEmpty(model.AccessToken))
                throw new ArgumentException("Access token is required");

            var userClaims = ValidateAndParseToken(model.AccessToken);
            if (userClaims == null)
                throw new UnauthorizedAccessException("Invalid Microsoft access token.");

            var email = userClaims.FindFirst(ClaimTypes.Email)?.Value ?? userClaims.FindFirst("preferred_username")?.Value;
            var name = userClaims.FindFirst(ClaimTypes.Name)?.Value;

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
        }

        private ClaimsPrincipal? ValidateAndParseToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // TODO: change to validate
                ValidIssuer = "https://login.microsoftonline.com/common/oauth2/v2.0",
                ValidateAudience = true,
                ValidAudiences = [authSettings.AzureAd.ClientId, $"api://{authSettings.AzureAd.ApplicationIdUri}"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                {
                    var keys = _httpClient.GetStringAsync("https://login.microsoftonline.com/common/discovery/v2.0/keys").Result;
                    return new JsonWebKeySet(keys).Keys;
                }
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
    }
}
