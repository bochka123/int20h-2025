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
        public DbSet<Integration> Integrations { get; set; }
        public DbSet<Prompt> Prompts { get; set; }
        public DbSet<PromptHistory> PromptHistories { get; set; }
        public DbSet<Entities.System> Systems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<Profile>(p => p.Id);

            modelBuilder.Entity<Integration>()
                .HasOne(i => i.Profile)
                .WithMany(u => u.Integrations)
                .HasForeignKey(i => i.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Integration>()
                .HasOne(i => i.System)
                .WithMany()
                .HasForeignKey(i => i.SystemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prompt>()
                .HasOne(p => p.Profile)
                .WithMany(u => u.Prompts)
                .HasForeignKey(p => p.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Prompt>()
                .HasOne(p => p.System)
                .WithMany()
                .HasForeignKey(p => p.SystemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PromptHistory>()
                .HasOne(ph => ph.Prompt)
                .WithMany(p => p.History)
                .HasForeignKey(ph => ph.PromptId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entities.System>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<Entities.System>()
                .Property(s => s.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Entities.System>()
                .Property(s => s.ApiBaseUrl)
                .HasMaxLength(255)
                .IsRequired();
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
