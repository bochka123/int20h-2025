using Int20h2025.DAL.Entities.Base;

namespace Int20h2025.DAL.Entities
{
    public class PromptHistory : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string Text { get; set; }
        public Guid PromptId { get; set; }
        public Prompt Prompt { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public bool IsCurrent { get; set; }
    }
}
