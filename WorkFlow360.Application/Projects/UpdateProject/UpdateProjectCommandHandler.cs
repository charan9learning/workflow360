using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Application.Common.Results;

namespace WorkFlow360.Application.Projects.UpdateProject
{
    public sealed class UpdateProjectCommandHandler
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IValidator<UpdateProjectCommand> _validator;
        public UpdateProjectCommandHandler(
            IApplicationDbContext dbContext,
            IValidator<UpdateProjectCommand> validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task<Result> HandleAsync(
            UpdateProjectCommand command,
            CancellationToken cancellationToken)
        {

            var validationResult =await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(
                    "; ",
                    validationResult.Errors.Select(
                        x => x.ErrorMessage));

                return Result.Failure(
                    Error.Validation(errorMessage));
            }

            var project = await _dbContext.Projects
          .FirstOrDefaultAsync(
              x => x.Id == command.Id,
              cancellationToken);


            if (project is null)
            {
                return Result.Failure(
                    Error.NotFound(
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
