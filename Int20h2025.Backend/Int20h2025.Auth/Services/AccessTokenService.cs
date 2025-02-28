using Int20h2025.Auth.Interfaces;
using Int20h2025.Auth.Models.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Int20h2025.Auth.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly SymmetricSecurityKey _symmetricSecurityKey;
        public AccessTokenService(AuthSettings authSettings)
        {
            _jwtSettings = authSettings.Jwt;
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            _symmetricSecurityKey = new SymmetricSecurityKey(hmac.Key);
        }
        public string GenerateAccessToken(Dictionary<string, object> claims)
        {

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenLifetimeMinutes),
                SigningCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Claims = claims
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool TryValidateAccessToken(string token, out ClaimsPrincipal? claimsPrincipal)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _symmetricSecurityKey,
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch
            {
                claimsPrincipal = null;
                return false;
            }
        }
    }
}
