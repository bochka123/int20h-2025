using Newtonsoft.Json;

namespace Int20h2025.Common.Models.DTO.Prompt
{
    public class PromptDTO
    {
        public string Text { get; set; } = string.Empty;
        public string? Result { get; set; }
        public bool Success { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(new
            {
                Prompt = Text,
                Result,
                Success
            }, Formatting.Indented);
        }
    }
}
