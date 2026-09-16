namespace WorkFlow360.Application.Projects.GetProjects
{
    public sealed record GetProjectsQuery(
        string? Search,
        int Page,
        int PageSize,
        string SortBy,
        string SortDirection);
}
