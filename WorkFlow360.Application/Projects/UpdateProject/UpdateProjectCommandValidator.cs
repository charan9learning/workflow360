using FluentValidation;

namespace WorkFlow360.Application.Projects.UpdateProject
{
    public sealed class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Project id is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Project name is required.")
                .MaximumLength(150)
                .WithMessage(
                    "Project name cannot exceed 150 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage(
                    "Project description cannot exceed 1000 characters.");
        }
    }
}
