using Int20h2025.Common.Models.DTO.Prompt;

namespace Int20h2025.BLL.Interfaces
{
    public interface IPromptService
    {
        Task<ICollection<PromptDTO>> GetHistoryAsync();
        Task CreateAsync(PromptDTO promptDTO);
    }
}
