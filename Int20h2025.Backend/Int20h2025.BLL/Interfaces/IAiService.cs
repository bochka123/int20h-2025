using Int20h2025.Common.Models.DTO.Ai;

namespace Int20h2025.BLL.Interfaces
{
    public interface IAiService
    {
        Task<Command> ProccessUserPromptAsync(string prompt, IEnumerable<string> historyMessages);
        Task<AiResponse> ProcessUserResponseAsync(bool ok, string response);
    }
}
