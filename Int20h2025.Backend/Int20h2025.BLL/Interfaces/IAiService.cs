using Int20h2025.Common.Models.DTO.Ai;

namespace Int20h2025.BLL.Interfaces
{
    public interface IAiService
    {
        Task<AiResponse> ProcessRequestAsync(AiRequest request);
    }
}
