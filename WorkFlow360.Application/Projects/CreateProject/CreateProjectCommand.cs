namespace WorkFlow360.Application.Projects.CreateProject
{
    public sealed record CreateProjectCommand(
      string Name,
      string? Description);
}
