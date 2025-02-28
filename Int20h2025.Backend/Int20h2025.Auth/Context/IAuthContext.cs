using Int20h2025.Auth.Entities;
using Microsoft.EntityFrameworkCore;

namespace Int20h2025.Auth.Context
{
    public interface IAuthContext
    {
        DbSet<User> Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
