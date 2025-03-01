using Int20h2025.Auth.Entities;
using Int20h2025.DAL.Entities.Base;

namespace Int20h2025.DAL.Entities
{
    public class Integration : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }
        public Guid SystemId { get; set; }
        public System System {  get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string ExpiresAt { get; set; }
        public bool IsConnected { get; set; }
    }
}
