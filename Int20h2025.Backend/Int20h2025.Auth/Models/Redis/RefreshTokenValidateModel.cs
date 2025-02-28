namespace Int20h2025.Auth.Models.Redis
{
    public class RefreshTokenValidateModel
    {
        public string Value { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
    }
}
