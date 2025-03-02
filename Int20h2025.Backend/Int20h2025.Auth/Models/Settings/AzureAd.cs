namespace Int20h2025.Auth.Models.Settings
{
    public class AzureAd
    {
        public string Audience { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
        public string ApplicationIdUri { get; set; }
        public string Scopes { get; set; }
    }
}
