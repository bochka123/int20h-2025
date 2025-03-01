using Int20h2025.BLL.Helpers;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Exceptions;
using Int20h2025.Common.Models.DTO.Ai;
using Newtonsoft.Json;
using OpenAI.Chat;

namespace Int20h2025.BLL.Services
{
    public class AiService(ChatClient client, IPromptService promptService) : IAiService
    {
        public async Task<AiResponse> ProcessRequestAsync(AiRequest request)
        {
            var messages = new List<ChatMessage>()
            {
                new SystemChatMessage(AiHelper.GeneralPrompt),
            };
            var history = await promptService.GetHistoryAsync();
            messages.AddRange(history.Select(x => new AssistantChatMessage(x.ToString())));
            messages.Add(new UserChatMessage(request.Prompt));
            var stringResp = await ReuqestAiAsync(messages);
            
            var command = JsonConvert.DeserializeObject<Command>(stringResp) 
                ?? throw new InternalPointerBobrException("Unknown ai error occured.");

            if (command.Clarification != null)
            {
                //await promptService.CreateAsync(new Common.Models.DTO.Prompt.PromptDTO
                //{

                //});
                return new AiResponse
                {
                    Message = command.Clarification
                };
            }

            var respMessage = "";
            messages.Add(new AssistantChatMessage(respMessage));
            var userResp = await ReuqestAiAsync(messages);
            //await promptService.CreateAsync();
            return new AiResponse
            {
                Message = userResp
            };
        }

        private async Task<string> ReuqestAiAsync(List<ChatMessage> messages)
        {
            var chatResponse = await client.CompleteChatAsync(messages);
            var stringResp = chatResponse.Value.Content.First().Text;
            if (string.IsNullOrEmpty(stringResp))
            {
                throw new InternalPointerBobrException("Answer from ai isn't parsed correctly.");
            }

            return stringResp;
        }
    }
}
