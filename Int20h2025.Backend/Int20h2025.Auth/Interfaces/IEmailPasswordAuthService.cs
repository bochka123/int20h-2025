using Int20h2025.Auth.Models.DTO;

namespace Int20h2025.Auth.Interfaces
{
    public interface IEmailPasswordAuthService
    {
        Task RegisterAsync(UserPasswordModel model);
        Task LoginAsync(UserPasswordModel model);
        void Logout();
    }
}
