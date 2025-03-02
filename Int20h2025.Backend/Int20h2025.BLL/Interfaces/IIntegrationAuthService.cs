namespace Int20h2025.BLL.Interfaces
{
    public interface IIntegrationAuthService
    {
        void SetCredentials(string apiKey, string token);
        object GetClient();
    }
}
