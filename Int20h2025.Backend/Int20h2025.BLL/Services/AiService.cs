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
        private readonly List<ChatMessage> ChatMessages = [aiHelper.GeneralPrompt];
        public async Task<Command> ProccessUserPromptAsync(string prompt, IEnumerable<string> historyMessages)
        {
            ChatMessages.AddRange(historyMessages.Select(x => new AssistantChatMessage(x)));
            ChatMessages.Add(new UserChatMessage(prompt));
            var stringResp = await ReuqestAiAsync()
                ?? throw new InternalPointerBobrException("Unknown ai error occured.");

            return JsonConvert.DeserializeObject<Command>(stringResp) ?? throw new InternalPointerBobrException("Unknown ai error occured.");
        }

        public async Task<AiResponse> ProcessUserResponseAsync(bool ok, string response)
        {
            var prompt = aiHelper.GetUserResponsePrompt(ok, response);
            ChatMessages.Add(new UserChatMessage(prompt));
            var userResp = await ReuqestAiAsync();

            var command =  JsonConvert.DeserializeObject<Command>(userResp) ?? throw new InternalPointerBobrException("Unknown ai error occured.");
            return new AiResponse
            {
                Message = command.Clarification
            };
        }

        private async Task<string> ReuqestAiAsync()
        {
            var chatResponse = await client.CompleteChatAsync(ChatMessages);
            var stringResp = chatResponse.Value.Content.First().Text;
            if (string.IsNullOrEmpty(stringResp))
            {
                throw new InternalPointerBobrException("Answer from ai isn't parsed correctly.");
            }

            return stringResp;
        }
    }
}
