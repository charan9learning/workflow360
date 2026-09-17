using WorkFlow360.Application.Common.Messaging;

namespace WorkFlow360.Application.Projects.UpdateProject
{
    public sealed record UpdateProjectCommand(
        Guid Id,
        string Name,
        string? Description): ICommand;
}
