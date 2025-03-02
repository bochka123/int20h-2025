using Int20h2025.Common.Enums;
using Int20h2025.Common.Models.DTO.IntegrationSystem;

namespace Int20h2025.BLL.Interfaces
{
    public interface IIntegrationService
    {
        Task IntegrateUserAsync(TaskManagersEnum integrationSystem);
        Task IntegrateUserAsync(IntegrationSystemDTO integrationSystem);
    }
}
