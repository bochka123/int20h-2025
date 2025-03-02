namespace Int20h2025.Common.Models.AzureDevops
{
    public class UpdateTaskModel
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public string? AssignedTo { get; set; } = null!;
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
    }
}
