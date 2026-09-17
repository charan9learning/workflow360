using Microsoft.EntityFrameworkCore;
using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Application.Common.Messaging;
using WorkFlow360.Application.Common.Results;
using WorkFlow360.Domain.Entities;

namespace WorkFlow360.Application.Projects.CreateProject
{
    public sealed class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, CreateProjectResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateProjectCommandHandler( IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<CreateProjectResponse>> Handle( CreateProjectCommand command, CancellationToken cancellationToken)
        {
            var projectExists = await _dbContext.Projects.AnyAsync( x => x.Name == command.Name, cancellationToken);

            if (projectExists)
            {
                return Result<CreateProjectResponse>.Failure( Error.Conflict( "Project.DuplicateName", "A project with this name already exists."));
            }

            var project = Project.Create( command.Name, command.Description);

            _dbContext.Projects.Add(project);

            await _dbContext.SaveChangesAsync( cancellationToken);

            var response =
                new CreateProjectResponse(
                    project.Id,
                    project.Name,
                    project.Description,
                    project.CreatedAtUtc);

            return Result<CreateProjectResponse>.Success(
                response);
        }
    }
}
