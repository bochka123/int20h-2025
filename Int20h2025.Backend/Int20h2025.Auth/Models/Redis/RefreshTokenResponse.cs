namespace Int20h2025.Auth.Models.Redis
{
    public class RefreshTokenResponse
    {
        public string Value { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
