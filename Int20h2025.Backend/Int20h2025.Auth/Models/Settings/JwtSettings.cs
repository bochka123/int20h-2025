namespace Int20h2025.Auth.Models.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenLifetimeMinutes { get; set; } = 30;
    }
}
