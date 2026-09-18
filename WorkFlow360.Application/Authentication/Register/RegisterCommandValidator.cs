using FluentValidation;

namespace WorkFlow360.Application.Authentication.Register
{
    public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(320).WithMessage("Email cannot exceed 320 characters.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must contain at least 8 characters.")
                .MaximumLength(128).WithMessage("Password cannot exceed 128 characters.");
        }
    }
}
