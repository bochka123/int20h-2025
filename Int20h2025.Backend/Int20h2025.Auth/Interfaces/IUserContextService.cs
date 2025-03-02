namespace Int20h2025.Auth.Interfaces
{
    public interface IUserContextService
    {
        Guid UserId { get; }
        public string? UserData { get; }
        internal void SetUser(Guid userId);
        internal void CacheData(string data);
    }
}
