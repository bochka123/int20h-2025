namespace Int20h2025.Common.Models.Api
{
    public class ApiResponse
    {
        public ApiResponse(bool success)
        {
            Ok = success;
        }
        public ApiResponse(bool success, string? message) : this(success)
        {
            Message = message;
        }

        public bool Ok { get; set; }
        public string? Message { get; set; }
    }
}
