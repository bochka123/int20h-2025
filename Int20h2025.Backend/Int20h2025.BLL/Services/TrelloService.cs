using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Models.Ai;
using Newtonsoft.Json.Linq;
using TrelloDotNet;
using TrelloDotNet.Model;

namespace Int20h2025.BLL.Services
{
    public class TrelloService(TrelloClient trelloClient) : ITaskManager
    {
        public string SystemName { get; init; } = "Trello";

        public async Task<OperationResult> ExecuteMethodAsync(string methodName, JObject parameters)
        {
            switch (methodName)
            {
                case "CreateTask":
                    var title = parameters["title"].ToString();
                    var description = parameters["description"].ToString();
                    var listId = parameters["listId"].ToString();
                    return await CreateTaskAsync(title, description, listId);

                case "UpdateTask":
                    var cardId = parameters["taskId"].ToString();
                    var newTitle = parameters["title"].ToString();
                    return await UpdateTaskAsync(cardId, newTitle);

                case "DeleteTask":
                    var deleteCardId = parameters["taskId"].ToString();
                    return await DeleteTaskAsync(deleteCardId);

                case "GetTasks":
                    var boardId = parameters["boardId"].ToString();
                    return await GetTasksAsync(boardId);

                case "GetTask":
                    var getCardId = parameters["taskId"].ToString();
                    return await GetTaskAsync(getCardId);

                default:
                    throw new NotImplementedException($"Method '{methodName}' is not implemented.");
            }
        }

        public SystemMethodInfo GetAvailableMethods()
        {
            return new SystemMethodInfo
            {
                SystemName = SystemName,
                Methods =
                [
                    new ServiceMethodInfo
                    {
                        MethodName = "CreateTask",
                        Description = "Creates a new card in the specified list.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "title", Type = "string", Description = "Card title." },
                            new ParameterInfo { Name = "description", Type = "string", Description = "Card description." },
                            new ParameterInfo { Name = "listId", Type = "string", Description = "List ID where the card will be created." }
                        ]
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "UpdateTask",
                        Description = "Updates the title of a card.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "taskId", Type = "string", Description = "Card ID to be updated." },
                            new ParameterInfo { Name = "title", Type = "string", Description = "New title." }
                        ]
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "DeleteTask",
                        Description = "Deletes a card.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "taskId", Type = "string", Description = "Card ID to be deleted." }
                        ]
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "GetTasks",
                        Description = "Retrieves all cards from a board.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "boardId", Type = "string", Description = "Board ID to fetch cards from." }
                        ]
                    },
                    new ServiceMethodInfo
                    {
                        MethodName = "GetTask",
                        Description = "Retrieves a single card by ID.",
                        Parameters =
                        [
                            new ParameterInfo { Name = "taskId", Type = "string", Description = "Card ID to fetch." }
                        ]
                    }
                ]
            };
        }

        private async Task<OperationResult> CreateTaskAsync(string title, string description, string listId)
        {
            var card = new Card(listId, title) { Description = description };
            await trelloClient.AddCardAsync(card);
            return new OperationResult { Response = $"Card '{title}' created successfully.", Success = true };
        }

        private async Task<OperationResult> UpdateTaskAsync(string cardId, string newTitle)
        {
            var card = await trelloClient.GetCardAsync(cardId);
            card.Name = newTitle;
            await trelloClient.UpdateCardAsync(card);
            return new OperationResult { Response = $"Card '{cardId}' updated successfully.", Success = true };
        }

        private async Task<OperationResult> DeleteTaskAsync(string cardId)
        {
            await trelloClient.DeleteCardAsync(cardId);
            return new OperationResult { Response = $"Card '{cardId}' deleted successfully.", Success = true };
        }

        private async Task<OperationResult> GetTasksAsync(string boardId)
        {
            var cards = await trelloClient.GetCardsOnBoardAsync(boardId);
            return new OperationResult { Response = JArray.FromObject(cards).ToString(), Success = true };
        }

        private async Task<OperationResult> GetTaskAsync(string cardId)
        {
            var card = await trelloClient.GetCardAsync(cardId);
            return new OperationResult { Response = JObject.FromObject(card).ToString(), Success = true };
        }
    }
}