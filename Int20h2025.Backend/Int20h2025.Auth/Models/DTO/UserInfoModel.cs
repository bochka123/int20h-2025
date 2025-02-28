using Newtonsoft.Json;

namespace Int20h2025.Auth.Models.DTO
{
    public class UserInfoModel
    {
        public string Email { get; set; }
        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }
    }
}
