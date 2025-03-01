using Int20h2025.BLL.Helpers;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Exceptions;
using Int20h2025.Common.Models.DTO.Ai;
using Newtonsoft.Json;
using OpenAI.Chat;

namespace Int20h2025.BLL.Services
{
    public class AiService(ChatClient client, AiHelper aiHelper) : IAiService
    {
        public async Task<AiResponse> ProcessRequestAsync(AiRequest request)
        {
            var messages = new List<ChatMessage>()
            {
                new SystemChatMessage(aiHelper.GeneralPrompt),
                new UserChatMessage(request.Prompt)
            };
            var chatResponse = await client.CompleteChatAsync(messages);
            var stringResp = chatResponse.Value.Content.First().Text;
            if (string.IsNullOrEmpty(stringResp))
            {
                throw new InternalPointerBobrException("Answer from ai isn't parsed correctly.");
            }
            var command = JsonConvert.DeserializeObject<Command>(stringResp) 
                ?? throw new InternalPointerBobrException("Unknown ai error occured.");

            if (command.Clarification != null)
            {
                return new AiResponse
                {
                    Clarification = command.Clarification
                };
            }

            return new AiResponse
            {
                Clarification = "lol"
            };
        }
    }
}
