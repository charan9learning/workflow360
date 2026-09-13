namespace WorkFlow360.Application.Projects.UpdateProject
{
    public sealed record UpdateProjectCommand(
        Guid Id,
        string Name,
        string? Description);
}
