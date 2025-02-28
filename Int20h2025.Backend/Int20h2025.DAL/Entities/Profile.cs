using Int20h2025.Auth.Entities;
using Int20h2025.DAL.Entities.Base;

namespace Int20h2025.DAL.Entities
{
    public class Profile : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public User User { get; set; } = null!;
    }
}
