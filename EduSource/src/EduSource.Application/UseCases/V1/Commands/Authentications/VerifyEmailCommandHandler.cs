using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Authentications;
using EduSource.Domain.Abstraction.Dappers;
using EduSource.Domain.Abstraction.EntitiyFramework;
using EduSource.Domain.Exceptions;
using MediatR;
using Newtonsoft.Json;

namespace EduSource.Application.UseCases.V1.Commands.Authentications;

public sealed class VerifyEmailCommandHandler : ICommandHandler<Command.VerifyEmailCommand>
{
    private readonly IResponseCacheService _responseCacheService;
    private readonly IEFUnitOfWork _efUnitOfWork;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IPublisher _publisher;

    public VerifyEmailCommandHandler(IResponseCacheService responseCacheService, IEFUnitOfWork efUnitOfWork, IPasswordHashService passwordHashService, IPublisher publisher)
    {
        _responseCacheService = responseCacheService;
        _efUnitOfWork = efUnitOfWork;
        _passwordHashService = passwordHashService;
        _publisher = publisher;
    }

    public async Task<Result> Handle(Command.VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        //Check if Email of User is existed
        var isExistedUserWithEmail = await _efUnitOfWork.AccountRepository.AnyAsync(acc => acc.Email == request.Email);
        if (isExistedUserWithEmail)
        {
            throw new AuthenticationException.EmailExistException();
        }

        //Get User Registered in Memory
        var registerMemory = await _responseCacheService.GetCacheResponseAsync($"register_{request.Email}");

        //Check if user is null
        if(registerMemory == null)
        {
            throw new AuthenticationException.RegisterTimeOutException();
        }

        //Convert Json data from memory to string data then convert string data to user data
        string unescapedJson = JsonConvert.DeserializeObject<string>(registerMemory);
        var user = JsonConvert.DeserializeObject<Command.RegisterCommand>(unescapedJson);

        //Hash password
        var passwordHash = _passwordHashService.HashPassword(user.Password);

        // Create object account with type register local
        var accountMember = Domain.Entities.Account.CreateMemberAccountLocal
            (user.FirstName, user.LastName, user.Email, user.PhoneNumber, passwordHash, user.Gender);

        _efUnitOfWork.AccountRepository.Add(accountMember);
        await _efUnitOfWork.SaveChangesAsync(cancellationToken);

        // Delete object saved in memory
        await _responseCacheService.DeleteCacheResponseAsync($"register_{request.Email}");

        // Send email when verified successfully
        await Task.WhenAll(
                _publisher.Publish(new DomainEvent.UserVerifiedEmailRegist(Guid.NewGuid(), request.Email),
                cancellationToken)
        );

        return Result.Success(new Success(MessagesList.VerifyEmailSuccess.GetMessage().Code,
            MessagesList.VerifyEmailSuccess.GetMessage().Message));

    }
}
