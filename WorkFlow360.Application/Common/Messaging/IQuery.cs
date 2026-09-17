using MediatR;
using WorkFlow360.Application.Common.Results;

namespace WorkFlow360.Application.Common.Messaging
{
    public interface IQuery<TResponse>
        : IRequest<Result<TResponse>>
    {
    }
}
