﻿using Int20h2025.BLL.Factories;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Models.DTO.Ai;

namespace Int20h2025.BLL.Services
{
    public class RequestProcessingService(IAiService aiService, IPromptService promptService, TaskManagerFactory taskManagerFactory) : IRequestProcessingService
    {
        public async Task<AiResponse> ProcessRequestAsync(AiRequest request)
        {
            var history = await promptService.GetHistoryAsync();
            var command = await aiService.ProccessUserPromptAsync(request.Prompt, history.Select(x => x.ToString()));
            if (command.Clarification != null)
            {
                await promptService.CreateAsync(new Common.Models.DTO.Prompt.PromptDTO
                {
                    Success = false,
                    Text = request.Prompt,
                    Result = command.Clarification
                });

                return new AiResponse
                {
                    Message = command.Clarification
                };
            }

            var taskManager = taskManagerFactory.GetTaskManager(Common.Enums.TaskManagersEnum.AzureDevOps);
            var response = await taskManager.ExecuteMethodAsync(command.Method, command.Parameters);
            var aiResp = await aiService.ProcessUserResponseAsync(response.Success, response.Response);
            await promptService.CreateAsync(new Common.Models.DTO.Prompt.PromptDTO
            {
                Text = request.Prompt,
                Result = aiResp.Message,
                Success = true
            });

            return aiResp;
        }
    }
}
