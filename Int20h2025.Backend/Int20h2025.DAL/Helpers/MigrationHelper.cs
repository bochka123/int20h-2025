using Int20h2025.DAL.Context;
using Int20h2025.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Int20h2025.Dal.Helpers
{
    public class MigrationHelper(Int20h2025Context context) : IMigrationHelper
    {
        public void Migrate()
        {
            if (context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
