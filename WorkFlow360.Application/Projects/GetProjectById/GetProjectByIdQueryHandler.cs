using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Application.Common.Results;

namespace WorkFlow360.Application.Projects.GetProjectById
{
    public sealed class GetProjectByIdQueryHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public GetProjectByIdQueryHandler(
            IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<ProjectResponse>> HandleAsync(
            GetProjectByIdQuery query,
            CancellationToken cancellationToken)
        {
            var project = await _dbContext.Projects
                .AsNoTracking()
                .Where(x => x.Id == query.Id)
                .Select(x => new ProjectResponse(
                    x.Id,
                    x.Name,
                    x.Description,
                    x.CreatedAtUtc,
                    x.UpdatedAtUtc))
                .FirstOrDefaultAsync(cancellationToken);

            if (project is null)
            {
                return Result<ProjectResponse>.Failure(
                    new Error(
                        "Project.NotFound",
                        "Project was not found."));
            }

            return Result<ProjectResponse>.Success(project);
        }
    }
}
