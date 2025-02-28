using Int20h2025.Auth.Context;
using Int20h2025.Auth.Entities;
using Int20h2025.DAL.Entities;
using Int20h2025.DAL.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Int20h2025.DAL.Context
{
    public class Int20h2025Context(DbContextOptions<Int20h2025Context> options) : DbContext(options), IAuthContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<Profile>(p => p.Id);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<IBaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.UpdatedOn = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedOn = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
