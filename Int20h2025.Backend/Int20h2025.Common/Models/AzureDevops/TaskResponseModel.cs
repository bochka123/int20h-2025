namespace Int20h2025.Common.Models.AzureDevops
{
    public class TaskResponseModel
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"TaskResponseModel: Id = {Id}, Url = {Url}, Title = {Title}";
        }
    }
}