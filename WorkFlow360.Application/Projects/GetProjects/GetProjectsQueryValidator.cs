using FluentValidation;

namespace WorkFlow360.Application.Projects.GetProjects
{
    public sealed class GetProjectsQueryValidator
      : AbstractValidator<GetProjectsQuery>
    {
        private static readonly string[] AllowedSortColumns =
        [
            "name",
        "createdAtUtc",
        "updatedAtUtc"
        ];

        public GetProjectsQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page must be greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage(
                    "Page size must be between 1 and 100.");

            RuleFor(x => x.SortBy)
                .Must(sortBy =>
                    AllowedSortColumns.Contains(
                        sortBy,
                        StringComparer.OrdinalIgnoreCase))
                .WithMessage(
                    "SortBy must be name, createdAtUtc or updatedAtUtc.");

            RuleFor(x => x.SortDirection)
                .Must(direction =>
                    direction.Equals(
                        "asc",
                        StringComparison.OrdinalIgnoreCase) ||
                    direction.Equals(
                        "desc",
                        StringComparison.OrdinalIgnoreCase))
                .WithMessage(
                    "SortDirection must be asc or desc.");

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage(
                    "Search cannot exceed 100 characters.");
        }
    }
}
