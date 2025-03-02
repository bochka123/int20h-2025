using Int20h2025.Auth.Attributes;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Models.Api;
using Int20h2025.Common.Models.DTO.Prompt;
using Microsoft.AspNetCore.Mvc;

namespace Int20h2025.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [MainAuth]
    public class PromptController(IPromptService promptService) : ControllerBase
    {
        [HttpGet]
        public async Task<ApiResponse<ICollection<PromptDTO>>> GetHistory() => new ApiResponse<ICollection<PromptDTO>>(await promptService.GetHistoryAsync());
    }
}
