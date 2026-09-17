using Microsoft.EntityFrameworkCore;
using WorkFlow360.Domain.Entities;

namespace WorkFlow360.Application.Common.Interface
{
    public interface IApplicationDbContext
    {
        DbSet<Project> Projects { get; }

        Task<int> SaveChangesAsync( CancellationToken cancellationToken = default);
    }
}
