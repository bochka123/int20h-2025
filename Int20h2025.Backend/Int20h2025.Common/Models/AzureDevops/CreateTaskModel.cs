namespace Int20h2025.Common.Models.AzureDevops
{
    public class CreateTaskModel
    {
        public string Title { get; set; } = null!;
        public string OrganizationName { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public string AssignedTo { get; set; } = null!;
        public string? Description { get; set; }
        public string? Priority { get; set; }
    }
}