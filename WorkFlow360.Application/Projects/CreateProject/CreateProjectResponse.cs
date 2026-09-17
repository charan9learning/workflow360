namespace WorkFlow360.Application.Projects.CreateProject
{
    public sealed record CreateProjectResponse( Guid Id, string Name, string? Description, DateTime CreatedAtUtc);
}
