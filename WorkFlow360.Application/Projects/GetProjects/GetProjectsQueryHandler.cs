using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Application.Common.Models;
using WorkFlow360.Application.Common.Results;
using WorkFlow360.Domain.Entities;

namespace WorkFlow360.Application.Projects.GetProjects
{
    public sealed class GetProjectsQueryHandler
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IValidator<GetProjectsQuery> _validator;

        public GetProjectsQueryHandler(
            IApplicationDbContext dbContext,
            IValidator<GetProjectsQuery> validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task<Result<PagedResult<ProjectListItemResponse>>> HandleAsync(
            GetProjectsQuery request,
            CancellationToken cancellationToken)
        {
            var validationResult =
                await _validator.ValidateAsync(
                    request,
                    cancellationToken);

            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(
                    "; ",
                    validationResult.Errors
                        .Select(x => x.ErrorMessage));

                return Result<PagedResult<ProjectListItemResponse>>
                    .Failure(
                        Error.Validation(errorMessage));
            }

            IQueryable<Project> query =
                _dbContext.Projects
                    .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(project =>
                    EF.Functions.Like(
                        project.Name,
                        $"%{search}%") ||
                    (project.Description != null &&
                     EF.Functions.Like(
                         project.Description,
                         $"%{search}%")));
            }

            query = ApplySorting(
                query,
                request.SortBy,
                request.SortDirection);

            var totalCount =
                await query.CountAsync(cancellationToken);

            var projects =
                await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(project =>
                        new ProjectListItemResponse(
                            project.Id,
                            project.Name,
                            project.Description,
                            project.CreatedAtUtc,
                            project.UpdatedAtUtc))
                    .ToListAsync(cancellationToken);

            var response =
                new PagedResult<ProjectListItemResponse>(
                    projects,
                    request.Page,
                    request.PageSize,
                    totalCount);

            return Result<PagedResult<ProjectListItemResponse>>
                .Success(response);
        }

        private static IQueryable<Project> ApplySorting(
            IQueryable<Project> query,
            string sortBy,
            string sortDirection)
        {
            var descending =
                sortDirection.Equals(
                    "desc",
                    StringComparison.OrdinalIgnoreCase);

            return sortBy.ToLowerInvariant() switch
            {
                "name" =>
                    descending
                        ? query.OrderByDescending(x => x.Name)
                        : query.OrderBy(x => x.Name),

                "createdatutc" =>
                    descending
                        ? query.OrderByDescending(x => x.CreatedAtUtc)
                        : query.OrderBy(x => x.CreatedAtUtc),

                "updatedatutc" =>
                    descending
                        ? query.OrderByDescending(x => x.UpdatedAtUtc)
                        : query.OrderBy(x => x.UpdatedAtUtc),

                _ => query.OrderBy(x => x.Name)
            };
        }
    }
}
