using Int20h2025.Auth.Interfaces;
using Int20h2025.Auth.Models.DTO;
using Int20h2025.Common.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace Int20h2025.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IEmailPasswordAuthService authService, IGoogleAuthService googleAuthService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] UserPasswordModel model)
        {
            await authService.RegisterAsync(model);
            //await accountService.EnsureProfileCreatedAsync();
            return Ok(new ApiResponse(true));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] UserPasswordModel model)
        {
            await authService.LoginAsync(model);
            return Ok(new ApiResponse(true));
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
            //await accountService.EnsureProfileCreatedAsync();
            return Ok(new ApiResponse(true));
        }
    }
}
