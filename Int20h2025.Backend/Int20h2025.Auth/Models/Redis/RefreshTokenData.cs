namespace Int20h2025.Auth.Models.Redis
{
    public class RefreshTokenData
    {
        public string Ip { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public Guid UserId { get; set; }
    }
}
