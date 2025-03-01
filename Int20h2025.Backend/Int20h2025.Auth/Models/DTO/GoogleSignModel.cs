using Int20h2025.Auth.Models.DTO.Base;

namespace Int20h2025.Auth.Models.DTO
{
    public class GoogleSignModel : IOAuthSignModel
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
