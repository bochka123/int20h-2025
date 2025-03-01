using Int20h2025.DAL.Entities.Base;

namespace Int20h2025.DAL.Entities
{
    public class Prompt : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; } = null!;
        public string Text { get; set; } = string.Empty;
        public string? Result { get; set; }
        public bool Success { get; set; }
        public List<PromptHistory> History { get; set; } = [];
    }
}
