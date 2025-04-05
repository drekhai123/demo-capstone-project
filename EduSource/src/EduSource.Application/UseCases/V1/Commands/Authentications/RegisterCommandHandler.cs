using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Authentications;
using EduSource.Domain.Abstraction.EntitiyFramework;
using EduSource.Domain.Exceptions;
using MediatR;
using System.Text.Json;

namespace EduSource.Application.UseCases.V1.Commands.Authentications;

public sealed class RegisterCommandHandler : ICommandHandler<Command.RegisterCommand>
{
    private readonly IResponseCacheService _responseCacheService;
    private readonly IEFUnitOfWork _efUnitOfWork;
    private readonly IPublisher _publisher;

    public RegisterCommandHandler(IResponseCacheService responseCacheService, IEFUnitOfWork efUnitOfWork, IPublisher publisher)
    {
        _responseCacheService = responseCacheService;
        _efUnitOfWork = efUnitOfWork;
        _publisher = publisher;
    }

    public async Task<Result> Handle(Command.RegisterCommand request, CancellationToken cancellationToken)
    {
        //Check if Email of User is existed
        var isExistedUserWithEmail = await _efUnitOfWork.AccountRepository.AnyAsync(acc => acc.Email == request.Email);
        if (isExistedUserWithEmail)
        {
            throw new AuthenticationException.EmailExistException();
        }

        //Save User Information Register to Redis then when User verify email, it will check with this information
        await _responseCacheService.SetCacheResponseAsync
            ($"register_{request.Email}",
            JsonSerializer.Serialize(request),
            TimeSpan.FromHours(12));

        // Send mail to notification user registed, and wait user accept
        await Task.WhenAll(
            _publisher.Publish(new DomainEvent.UserRegistedWithLocal(Guid.NewGuid(), request.Email), cancellationToken)
        );

        //Return success result
        return Result.Success(new Success(MessagesList.AuthRegisterSuccess.GetMessage().Code, MessagesList.AuthRegisterSuccess.GetMessage().Message));

    }
}
