using Int20h2025.Auth.Attributes;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Models.Api;
using Int20h2025.Common.Models.DTO.Profile;
using Microsoft.AspNetCore.Mvc;

namespace Int20h2025.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [MainAuth]
    public class ProfileController(IProfileService profileService) : ControllerBase
    {
        [HttpGet("myprofile")]
        public async Task<ApiResponse<ProfileDTO>> GetMyProfile() => new ApiResponse<ProfileDTO>(await profileService.GetMyProfileAsync());
    }
}
