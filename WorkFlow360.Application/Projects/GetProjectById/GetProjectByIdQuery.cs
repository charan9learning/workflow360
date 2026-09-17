using WorkFlow360.Application.Common.Messaging;

namespace WorkFlow360.Application.Projects.GetProjectById
{
    public sealed record GetProjectByIdQuery(
       Guid Id):IQuery<ProjectResponse>;
}
