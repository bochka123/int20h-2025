using Int20h2025.Auth.Attributes;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Models.Api;
using Int20h2025.Common.Models.DTO.Ai;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Int20h2025.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [MainAuth]
    public class AiController(IRequestProcessingService processingService) : ControllerBase
    {
        [HttpPost("process")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] AiRequest model)
            => Ok(new ApiResponse<AiResponse>(await processingService.ProcessRequestAsync(model)));
    }
}
