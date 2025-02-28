using Int20h2025.Auth.Entities;
using Int20h2025.Auth.Interfaces;
using Int20h2025.Auth.Models.Redis;
using Int20h2025.Auth.Models.Settings;
using Microsoft.AspNetCore.Http;

namespace Int20h2025.Auth.Services
{
    public class CookieAuthService(
        IAccessTokenService accessTokenService,
        IRefreshTokenService refreshTokenService,
        AuthSettings authSettings,
        IUserContextService userContextService,
        IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        private readonly CookieSettings cookieSettings = authSettings.Cookie;

        public async Task<bool> ValidateRequestAsync(HttpContext context)
        {
            var accessToken = context.Request.Cookies[cookieSettings.AccessTokenName];
            var refreshToken = context.Request.Cookies[cookieSettings.RefreshTokenName];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return false;
            }

            Guid userId;
            if (!string.IsNullOrEmpty(accessToken) && accessTokenService.TryValidateAccessToken(accessToken, out var claimsPrincipal))
            {
                var userIdClaim = claimsPrincipal?.FindFirst(nameof(User))?.Value;
                if (userIdClaim == null || !Guid.TryParse(userIdClaim, out userId))
                {
                    return false;
                }

                userContextService.SetUser(userId);
                return true;
            }

            var validateModel = new RefreshTokenValidateModel
            {
                Ip = context.Connection.RemoteIpAddress.ToString(),
                Value = refreshToken
            };
            var storedToken = await refreshTokenService.GetValidatedRefreshTokenAsync(validateModel);
            if (storedToken == null)
            {
                return false;
            }
            userId = storedToken.UserId;
            userContextService.SetUser(userId);
            SetRefreshToken(storedToken.Value);
            SetAccessToken(userId);

            return true;
        }

        public void Logout()
        {
            var context = httpContextAccessor.HttpContext;
            if (context == null)
            {
                return;
            }

            context.Response.Cookies.Delete(cookieSettings.RefreshTokenName, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            });

            context.Response.Cookies.Delete(cookieSettings.AccessTokenName, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            });
        }

        public void SetupAuth(Guid userId)
        {
            userContextService.SetUser(userId);
            var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var refreshToken = refreshTokenService.GenerateRefreshToken(userId, ip);
            SetRefreshToken(refreshToken.Value);
            SetAccessToken(userId);
        }

        private void SetAccessToken(Guid userId)
        {
            var claims = new Dictionary<string, object>()
            {
                { nameof(User), userId.ToString() },
            };

            var newAccessToken = accessTokenService.GenerateAccessToken(claims);
            SetCookie(cookieSettings.AccessTokenName, newAccessToken, DateTime.UtcNow.AddMinutes(cookieSettings.AccessTokenCookieMinutesExpire));
        }

        private void SetRefreshToken(string value) =>
            SetCookie(cookieSettings.RefreshTokenName, value, DateTime.UtcNow.AddDays(cookieSettings.RefreshTokenCookieDaysExpire));

        private void SetCookie(string key, string value, DateTimeOffset timeOffset)
        {
            var context = httpContextAccessor.HttpContext;
            if (context == null)
            {
                return;
            }
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = timeOffset
            };

            context.Response.Cookies.Append(key, value, options);
        }
    }
}