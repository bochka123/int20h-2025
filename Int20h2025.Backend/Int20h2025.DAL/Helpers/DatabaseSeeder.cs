using Int20h2025.Common.Enums;
using Int20h2025.DAL.Context;
using Int20h2025.DAL.Interfaces;

namespace Int20h2025.DAL.Helpers
{
    public class DatabaseSeeder(Int20h2025Context context) : IDatabaseSeeder
    {
        private readonly Int20h2025Context _context = context;

        public async Task SeedAsync()
        {
            if (!_context.Systems.Any())
            {
                var systems = new List<Entities.System>
                {
                    new() { Id = Guid.NewGuid(), Name = nameof(TaskManagersEnum.AzureDevOps), Description = "", ApiBaseUrl = "https://dev.azure.com/" },
                    new() { Id = Guid.NewGuid(), Name = nameof(TaskManagersEnum.Trello), Description = "", ApiBaseUrl = "" },
                };

                _context.Systems.AddRange(systems);
                await _context.SaveChangesAsync();
            }
        }
    }
}
