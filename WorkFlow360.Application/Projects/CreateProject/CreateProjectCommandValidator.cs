using FluentValidation;

namespace WorkFlow360.Application.Projects.CreateProject
{
    public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
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
