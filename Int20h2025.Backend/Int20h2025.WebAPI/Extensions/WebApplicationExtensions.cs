using Int20h2025.DAL.Interfaces;

namespace Int20h2025.WebAPI.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void SeedDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var migrationHelper = scope.ServiceProvider.GetRequiredService<IMigrationHelper>();
            migrationHelper.Migrate();

            var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
            seeder.SeedAsync().GetAwaiter().GetResult();
        }
    }
}
