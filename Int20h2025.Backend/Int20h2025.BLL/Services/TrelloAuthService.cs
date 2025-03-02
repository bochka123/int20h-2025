using Int20h2025.BLL.Interfaces;
using TrelloDotNet;

namespace Int20h2025.BLL.Services
{
    public class TrelloAuthService : ITrelloAuthService
    {
        private string? _apiKey;
        private string? _token;

        public void SetCredentials(string apiKey, string token)
        {
            _apiKey = apiKey;
            _token = token;
        }

        public object GetClient()
        {
            if (string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_token))
                throw new InvalidOperationException("Trello API credentials are not set.");

            return new TrelloClient(_apiKey, _token);
        }
    }
}
