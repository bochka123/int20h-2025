using Int20h2025.Auth.Models.Settings;

namespace Int20h2025.BLL.Settings
{
    public class AppSettings
    {
        public AuthSettings Auth { get; set; } = null!;
        public AiSettings Ai { get; set; } = null!;
    }
}
