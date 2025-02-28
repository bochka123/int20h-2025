using Int20h2025.DAL.Context;
using Int20h2025.DAL.Interfaces;

namespace Int20h2025.DAL.Helpers
{
    public class DatabaseSeeder(Int20h2025Context context) : IDatabaseSeeder
    {
        private readonly Int20h2025Context _context = context;

        public async Task SeedAsync()
        {
            
        }
    }
}
