using Int20h2025.Auth.Entities;
using Int20h2025.DAL.Entities.Base;

namespace Int20h2025.DAL.Entities
{
    public class Prompt : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }
        public Guid SystemId { get; set; }
        public System System {  get; set; }
        public string Text { get; set; }
        public List<PromptHistory> History { get; set; } 
    }
}
