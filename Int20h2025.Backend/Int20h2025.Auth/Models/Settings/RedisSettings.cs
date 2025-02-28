namespace Int20h2025.Auth.Models.Settings
{
    public class RedisSettings
    {
        public string ConnectionString { get; set; }
        public int DaysToExpire { get; set; } = 7;
    }
}
