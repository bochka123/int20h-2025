using Newtonsoft.Json.Linq;

namespace Int20h2025.Common.Models.DTO.Ai
{
    public class Command
    {
        public string? System { get; set; }
        public string Method { get; set; } = string.Empty;
        public JObject Parameters { get; set; } = [];
        public string? Clarification { get; set; }
    }
}
