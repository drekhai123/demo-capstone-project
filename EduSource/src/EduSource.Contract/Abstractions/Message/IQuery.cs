using MediatR;
using EduSource.Contract.Abstractions.Shared;

namespace EduSource.Contract.Abstractions.Message;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}