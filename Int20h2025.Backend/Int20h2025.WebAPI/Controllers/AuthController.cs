using Int20h2025.Auth.Interfaces;
using Int20h2025.Auth.Models.DTO;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Models.Api;
using Int20h2025.Common.Models.DTO.Profile;
using Microsoft.AspNetCore.Mvc;

namespace Int20h2025.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IEmailPasswordAuthService authService, IGoogleAuthService googleAuthService, IMicrosoftAuthService microsoftAuthService, IProfileService profileService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] UserPasswordModel model)
        {
            await authService.RegisterAsync(model);
            var profile = await profileService.EnsureProfileCreatedAsync();
            return Ok(new ApiResponse<ProfileDTO>(profile));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] UserPasswordModel model)
        {
            await authService.LoginAsync(model);
            var profile = await profileService.EnsureProfileCreatedAsync();
            return Ok(new ApiResponse<ProfileDTO>(profile));
        }

        [HttpPost("logout")]
        public ActionResult<ApiResponse> Logout()
        {
            authService.Logout();
            return Ok(new ApiResponse(true));
        }

        [HttpPost("google")]
        public async Task<ActionResult<ApiResponse>> Google([FromBody] GoogleSignModel model)
        {
            await googleAuthService.SignInAsync(model);
            var profile = await profileService.EnsureProfileCreatedAsync();
            return Ok(new ApiResponse<ProfileDTO>(profile));
        }

        [HttpPost("microsoft")]
        public async Task<ActionResult<ApiResponse>> Microsoft([FromBody] MicrosoftSignModel model)
        {
            await microsoftAuthService.SignInAsync(model);
            var profile = await profileService.EnsureProfileCreatedAsync();
            return Ok(new ApiResponse<ProfileDTO>(profile));
        }
    }
}
