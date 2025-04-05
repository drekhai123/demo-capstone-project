using MediatR;
using EduSource.Contract.Abstractions.Shared;

namespace EduSource.Contract.Abstractions.Message;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
