using Microsoft.EntityFrameworkCore;
using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Application.Common.Results;

namespace WorkFlow360.Application.Projects.DeleteProject
{
    public sealed class DeleteProjectCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteProjectCommandHandler(
            IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> HandleAsync(
            DeleteProjectCommand command,
            CancellationToken cancellationToken)
        {
            var project = await _dbContext.Projects
                                 .FirstOrDefaultAsync(
                                    x => x.Id == command.Id,
                                           cancellationToken);

            if (project is null)
            {
                return Result.Failure(
                    new Error(
                        "Project.NotFound",
                        "Project was not found."));
            }

            _dbContext.Projects.Remove(project);

            await _dbContext.SaveChangesAsync(
                cancellationToken);

            return Result.Success();
        }
    }
}
