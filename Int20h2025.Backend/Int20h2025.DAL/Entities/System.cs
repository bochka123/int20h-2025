using Int20h2025.DAL.Entities.Base;

namespace Int20h2025.DAL.Entities
{
    public class System : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ApiBaseUrl { get; set; }
    }
}
