using Int20h2025.Auth.Models.DTO;

namespace Int20h2025.Auth.Interfaces
{
    public interface IGoogleAuthService
    {
        public Task SignInAsync(GoogleSignModel model);
    }
}
