using Int20h2025.Auth.Context;
using Int20h2025.Auth.Entities;
using Int20h2025.Auth.Interfaces;
using Int20h2025.Auth.Models.DTO;
using Int20h2025.Auth.Models.Settings;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Authentication;

namespace Int20h2025.Auth.Services
{
    public class GoogleAuthService(AuthSettings authSettings, HttpClient _httpClient, IAuthContext context, IAuthService authService) : IGoogleAuthService
    {
        public async Task SignInAsync(GoogleSignModel model)
        {
            try
            {
                var creds = GoogleCredential.FromAccessToken(model.AccessToken);
                var response = await _httpClient.GetAsync($"{authSettings.Google.UserInfoUrl}?access_token={model.AccessToken}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new UnauthorizedAccessException("Invalid Google access token.");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var payload = JsonConvert.DeserializeObject<UserInfoModel>(jsonResponse)!;

                if (!payload.EmailVerified)
                {
                    throw new InvalidCredentialException("Email is not verified.");
                }
                var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == payload.Email);
                if (user == null)
                {
                    user = new User
                    {
                        Email = payload.Email
                    };

                    await context.Users.AddAsync(user);
                    await context.SaveChangesAsync();
                }

                authService.SetupAuth(user.Id);
            }
            catch (InvalidJwtException)
            {
                throw new UnauthorizedAccessException("Invalid Google token.");
            }
        }
    }
}
