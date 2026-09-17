using WorkFlow360.Application.Common.Messaging;

namespace WorkFlow360.Application.Projects.DeleteProject
{
    public sealed record DeleteProjectCommand( Guid Id): ICommand;
}
