using MediatR;
using WorkFlow360.Application.Common.Results;

namespace WorkFlow360.Application.Common.Messaging
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
    {
    }
}
