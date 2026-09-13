using Microsoft.EntityFrameworkCore;
using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Application.Common.Results;

namespace WorkFlow360.Application.Projects.UpdateProject
{
    public sealed class UpdateProjectCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdateProjectCommandHandler(
            IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> HandleAsync(
            UpdateProjectCommand command,
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

            project.Update(
                command.Name,
                command.Description);

            await _dbContext.SaveChangesAsync(
                cancellationToken);

            return Result.Success();
        }
    }
}
