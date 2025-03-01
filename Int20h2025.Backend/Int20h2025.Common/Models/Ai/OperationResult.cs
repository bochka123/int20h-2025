namespace Int20h2025.Common.Models.Ai
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Response { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"You are an AI assistant. Your task is to generate a concise and helpful response for the user based on the following data:\n\n" +
                   $"Success: {Success}\n" +
                   $"Response: {Response}\n\n" +
                   "Instructions:\n" +
                   "1. If 'Success' is true, analyze the 'Response' and provide a useful summary or answer for the user.\n" +
                   "2. If 'Success' is false, explain the issue in a user-friendly way and suggest possible solutions.\n" +
                   "3. Always respond in a single, concise sentence.";
        }
    }
}