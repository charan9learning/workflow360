using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Application.Common.Results;
using WorkFlow360.Domain.Entities;

namespace WorkFlow360.Application.Projects.CreateProject
{
    public sealed class CreateProjectCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IValidator<CreateProjectCommand> _validator;

        public CreateProjectCommandHandler(
            IApplicationDbContext dbContext,
            IValidator<CreateProjectCommand> validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task<Result<CreateProjectResponse>> HandleAsync(
            CreateProjectCommand command,
            CancellationToken cancellationToken)
        {
            var projectExists =
                 await _dbContext.Projects
                                    .AnyAsync(
                                        x => x.Name == command.Name,
                                        cancellationToken);

            if (projectExists)
            {
                return Result<CreateProjectResponse>.Failure(
                    Error.Conflict(
                        "Project.DuplicateName",
                        "A project with this name already exists."));
            }

            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ",validationResult.Errors.Select(x => x.ErrorMessage));

                return Result<CreateProjectResponse>.Failure(Error.Validation(errorMessage));
            }

            var project = Project.Create(
                command.Name,
                command.Description);

            _dbContext.Projects.Add(project);

            await _dbContext.SaveChangesAsync(
                cancellationToken);

            var response = new CreateProjectResponse(
                project.Id,
                project.Name,
                project.Description,
                project.CreatedAtUtc);

            return Result<CreateProjectResponse>.Success(response);

        }
    }
}
