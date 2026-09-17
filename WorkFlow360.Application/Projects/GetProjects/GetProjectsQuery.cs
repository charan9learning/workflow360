using WorkFlow360.Application.Common.Messaging;
using WorkFlow360.Application.Common.Models;

namespace WorkFlow360.Application.Projects.GetProjects
{
    public sealed record GetProjectsQuery(
        string? Search,
        int Page,
        int PageSize,
        string SortBy,
        string SortDirection) : IQuery<PagedResult<ProjectListItemResponse>>;
}
