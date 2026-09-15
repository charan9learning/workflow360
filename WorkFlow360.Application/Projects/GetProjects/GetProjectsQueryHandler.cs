using Microsoft.EntityFrameworkCore;
using WorkFlow360.Application.Common.Interface;

namespace WorkFlow360.Application.Projects.GetProjects
{
    public sealed class GetProjectsQueryHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public GetProjectsQueryHandler(
            IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<ProjectListItemResponse>> HandleAsync(
            GetProjectsQuery query,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Projects
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ProjectListItemResponse(
                    x.Id,
                    x.Name,
                    x.Description,
                    x.CreatedAtUtc,
                    x.UpdatedAtUtc))
                .ToListAsync(cancellationToken);
        }
    }
}
