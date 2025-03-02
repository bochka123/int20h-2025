using Int20h2025.Auth.Attributes;
using Int20h2025.BLL.Factories;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Models.Api;
using Int20h2025.Common.Models.DTO.IntegrationSystem;
using Microsoft.AspNetCore.Mvc;

namespace Int20h2025.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [MainAuth]
    public class IntegrationController(IIntegrationService integrationService) : ControllerBase
    {
        [HttpPost("integrate")]
        public async Task<ActionResult<ApiResponse>> Integrate([FromBody] IntegrationSystemDTO model)
        {
            await integrationService.IntegrateUserAsync(model);
            var credsService = HttpContext.RequestServices.GetRequiredService<CredsRegisterFactory>()
                    .RegisterCreds(model.SystemName);

            credsService.SetCredentials(model.ApiKey, model.Token);
            return Ok(new ApiResponse(true));
        }

        [HttpPost("check")]
        public async Task<ActionResult<ApiResponse>> Check([FromBody] CheckIntegrationSystemDTO model) => 
            Ok(new ApiResponse(await integrationService.CheckIntegrationAsync(model)));
    }
}
