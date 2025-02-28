using Int20h2025.Common.Models.DTO.Profile;

namespace Int20h2025.BLL.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileDTO> EnsureProfileCreatedAsync();
    }
}
