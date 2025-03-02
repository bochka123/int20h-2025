namespace Int20h2025.Common.Models.AzureDevops
{
    public class DeleteTaskModel
    {
        public int TaskId { get; set; }
        public string OrganizationName { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
    }
}
