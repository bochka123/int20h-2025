using Int20h2025.Auth.Interfaces;

namespace Int20h2025.Auth.Services
{
    public class UserContextService: IUserContextService
    {
        public Guid UserId { get; private set; }

        void IUserContextService.SetUser(Guid userId)
        {
            UserId = userId;
        }
    }
}
