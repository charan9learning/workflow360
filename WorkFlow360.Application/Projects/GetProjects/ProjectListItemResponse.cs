namespace WorkFlow360.Application.Projects.GetProjects
{
    public sealed record ProjectListItemResponse(
       Guid Id,
       string Name,
       string? Description,
       DateTime CreatedAtUtc,
       DateTime? UpdatedAtUtc);
}
