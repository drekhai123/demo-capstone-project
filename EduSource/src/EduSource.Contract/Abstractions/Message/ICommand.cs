using MediatR;
using EduSource.Contract.Abstractions.Shared;

namespace EduSource.Contract.Abstractions.Message;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}