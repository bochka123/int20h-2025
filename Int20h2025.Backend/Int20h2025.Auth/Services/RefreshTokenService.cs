using Int20h2025.Auth.Interfaces;
using Int20h2025.Auth.Models.Redis;
using Int20h2025.Auth.Models.Settings;
using Int20h2025.Auth.Utilities;
using Newtonsoft.Json;

namespace Int20h2025.Auth.Services
{
    public class RefreshTokenService(AuthSettings authSettings, IRefreshTokenStore tokenStore) : IRefreshTokenService
    {
        private readonly RefreshTokenSettings refreshSettings = authSettings.RefreshToken;
        public RefreshTokenResponse GenerateRefreshToken(Guid userId, string ip)
        {
            var refreshToken = new RefreshTokenData
            {
                UserId = userId,
                Ip = ip,
                Expiration = DateTime.UtcNow.AddDays(refreshSettings.TokenLifeTimeInDays)
            };
            var id = Guid.NewGuid().ToString();
            var token = TokenEncryptionUtility.EncryptToken(id, refreshSettings.Secret);
            var encryptedData = TokenEncryptionUtility.EncryptToken(JsonConvert.SerializeObject(refreshToken), id);

            tokenStore.SaveRefreshTokenAsync(token, encryptedData);

            return new RefreshTokenResponse
            {
                UserId = userId,
                Value = token
            };
        }

        public async Task<RefreshTokenResponse?> GetValidatedRefreshTokenAsync(RefreshTokenValidateModel model)
        {
            var encryptedTokenData = await tokenStore.GetRefreshTokenAsync(model.Value);
            if (encryptedTokenData != null)
            {
                if (TokenEncryptionUtility.TryDecryptToken(model.Value, refreshSettings.Secret, out var id))
                {
                    if (TokenEncryptionUtility.TryDecryptToken(encryptedTokenData, id!, out var stringTokenData))
                    {
                        var tokenData = JsonConvert.DeserializeObject<RefreshTokenData>(stringTokenData!);
                        if (tokenData!.Ip == model.Ip && tokenData.Expiration > DateTime.UtcNow)
                        {
                            return GenerateRefreshToken(tokenData.UserId, model.Ip);
                        }
                    }
                }
            }

            return null;
        }
    }
}
