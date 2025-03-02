using Int20h2025.Common.Enums;

namespace Int20h2025.Common.Models.DTO.IntegrationSystem
{
    public class IntegrationSystemDTO
    {
        public TaskManagersEnum SystemName { get; set; }
        public string ApiKey { get; set; }
        public string Token { get; set; }
    }
}
