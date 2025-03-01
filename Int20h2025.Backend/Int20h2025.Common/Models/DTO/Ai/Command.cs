namespace Int20h2025.Common.Models.DTO.Ai
{
    public class Command
    {
        public string? System { get; set; }
        public string? Method { get; set; }
        public object[] Parameters { get; set; } = [];
        public string? Clarification { get; set; }
    }
}
