namespace WorkFlow360.Application.Projects.GetProjectById
{
    public sealed record ProjectResponse( Guid Id, string Name, string? Description, DateTime CreatedAtUtc, DateTime? UpdatedAtUtc);
}
