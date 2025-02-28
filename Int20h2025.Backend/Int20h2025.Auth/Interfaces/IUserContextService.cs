namespace Int20h2025.Auth.Interfaces
{
    public interface IUserContextService
    {
        Guid UserId { get; }
        internal void SetUser(Guid userId);
    }
}
