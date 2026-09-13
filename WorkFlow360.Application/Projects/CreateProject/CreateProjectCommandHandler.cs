using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Domain.Entities;

namespace WorkFlow360.Application.Projects.CreateProject
{
    public sealed class CreateProjectCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateProjectCommandHandler(
            IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateProjectResponse> HandleAsync(
            CreateProjectCommand command,
            CancellationToken cancellationToken)
        {
            var project = Project.Create(
                command.Name,
                command.Description);

            _dbContext.Projects.Add(project);

            await _dbContext.SaveChangesAsync(
                cancellationToken);

            return new CreateProjectResponse(
                project.Id,
                project.Name,
                project.Description,
                project.CreatedAtUtc);
        }
    }
}
