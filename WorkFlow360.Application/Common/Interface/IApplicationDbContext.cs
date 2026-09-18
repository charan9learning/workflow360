using Microsoft.EntityFrameworkCore;
using WorkFlow360.Domain.Entities;

namespace WorkFlow360.Application.Common.Interface
{
    public interface IApplicationDbContext
    {
        DbSet<Project> Projects { get; }
        DbSet<User> Users { get; }
        Task<int> SaveChangesAsync( CancellationToken cancellationToken = default);
    }
}
