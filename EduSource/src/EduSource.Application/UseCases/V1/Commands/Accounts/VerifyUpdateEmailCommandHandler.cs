using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Accounts;
using EduSource.Domain.Abstraction.EntitiyFramework;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using Newtonsoft.Json;
using static EduSource.Domain.Exceptions.AccountException;


namespace EduSource.Application.UseCases.V1.Commands.Account;

public sealed class VerifyUpdateEmailCommandHandler : ICommandHandler<Command.VerifyUpdateEmailCommand, Success>
{
    private readonly IResponseCacheService _responseCacheService;
    private readonly IRepositoryBase<EduSource.Domain.Entities.Account, Guid> _accountRepository;
    private readonly IEFUnitOfWork _efUnitOfWork;

    public VerifyUpdateEmailCommandHandler
        (IResponseCacheService responseCacheService,
        IRepositoryBase<EduSource.Domain.Entities.Account, Guid> accountRepository,
        IEFUnitOfWork efUnitOfWork)
    {
        _responseCacheService = responseCacheService;
        _accountRepository = accountRepository;
        _efUnitOfWork = efUnitOfWork;
    }

    public async Task<Result<Success>> Handle(Command.VerifyUpdateEmailCommand request, CancellationToken cancellationToken)
    {
        var changeEmailMemory = await _responseCacheService.GetCacheResponseAsync($"changeemail_{request.UserId}");
        var newEmail = JsonConvert.DeserializeObject<string>(changeEmailMemory);
        var user = await _accountRepository.FindByIdAsync(request.UserId);
        if (user == null) throw new AccountNotFoundException();
        
        user.UpdateEmail(newEmail);
        await _efUnitOfWork.SaveChangesAsync(cancellationToken);

        await _responseCacheService.DeleteCacheResponseAsync($"changeemail_{request.UserId}");
        
        return Result.Success(new Success(MessagesList.AccountUpdateEmailSuccess.GetMessage().Code,
            MessagesList.AccountUpdateEmailSuccess.GetMessage().Message
            ));
    }
}
