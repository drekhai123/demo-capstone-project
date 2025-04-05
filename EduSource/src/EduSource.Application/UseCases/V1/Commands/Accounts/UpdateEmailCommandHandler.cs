using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.Authentication;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Accounts;
using EduSource.Domain.Abstraction.EntitiyFramework;
using MediatR;
using static EduSource.Domain.Exceptions.AccountException;
namespace EduSource.Application.UseCases.V1.Commands.Account;

public sealed class UpdateEmailCommandHandler : ICommandHandler<Command.UpdateEmailCommand, Success>
{
    private readonly IPublisher _publisher;
    private readonly IResponseCacheService _responseCacheService;
    private readonly IEFUnitOfWork _efUnitOfWork;

    public UpdateEmailCommandHandler(IPublisher publisher,
        IResponseCacheService responseCacheService,
        IEFUnitOfWork efUnitOfWork)
    {
        _publisher = publisher;
        _responseCacheService = responseCacheService;
        _efUnitOfWork = efUnitOfWork;
    }

    public async Task<Result<Success>> Handle(Command.UpdateEmailCommand request, CancellationToken cancellationToken)
    {
        var isCheckMail = await _efUnitOfWork.AccountRepository.AnyAsync(account => account.Email == request.Email);
        if (isCheckMail == true)
            throw new AccountUpdateEmailExit();

        var account = await _efUnitOfWork.AccountRepository.FindByIdAsync(request.UserId);
        if (account == null)
            throw new AccountEmailDuplicateException();

        if(account.LoginType != LoginType.Local)
            throw new AccountNotLoginLocalException();


        await _responseCacheService.SetCacheResponseAsync($"changeemail_{request.UserId}", request.Email, TimeSpan.FromMinutes(30));
        await Task.WhenAll(
            _publisher.Publish(new DomainEvent.UserEmailChanged(Guid.NewGuid(), request.UserId, account.Email), cancellationToken)
        );
        
        return Result.Success(new Success(MessagesList.AccountUpdateChangeEmail.GetMessage().Code, MessagesList.AccountUpdateChangeEmail.GetMessage().Message));
    }
}
