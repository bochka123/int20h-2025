namespace Int20h2025.Auth.Models.Settings
{
    public class AuthSettings
    {
        public GoogleSettings Google {  get; set; }
        public RedisSettings Redis { get; set; }
        public CookieSettings Cookie { get; set; }
        public JwtSettings Jwt { get; set; }
        public RefreshTokenSettings RefreshToken { get; set; }
    }
}
