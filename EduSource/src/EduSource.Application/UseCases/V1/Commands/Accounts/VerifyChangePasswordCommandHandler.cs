using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Services;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Accounts;
using EduSource.Domain.Abstraction.EntitiyFramework;
using Newtonsoft.Json;
using static EduSource.Domain.Exceptions.AccountException;


namespace EduSource.Application.UseCases.V1.Commands.Account;

public sealed class VerifyChangePasswordCommandHandler : ICommandHandler<Command.VerifyChangePasswordCommand, Success>
{
    private readonly IEFUnitOfWork _efUnitOfWork;
    private readonly IResponseCacheService _responseCacheService;

    public VerifyChangePasswordCommandHandler
        (IEFUnitOfWork efUnitOfWork,
        IResponseCacheService responseCacheService)
    {
        _efUnitOfWork = efUnitOfWork;
        _responseCacheService = responseCacheService;
    }

    public async Task<Result<Success>> Handle(Command.VerifyChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _efUnitOfWork.AccountRepository.FindByIdAsync(request.UserId);
        if (user == null) throw new AccountNotFoundException();
        var changePasswordMemory = await _responseCacheService.GetCacheResponseAsync($"changepassword_{request.UserId}");
        var newPassword = JsonConvert.DeserializeObject<string>(changePasswordMemory);

        user.UpdatePassword(newPassword);
        await _efUnitOfWork.SaveChangesAsync(cancellationToken);
        
        await _responseCacheService.DeleteCacheResponseAsync($"changepassword_{request.UserId}");

        return Result.Success(new Success(MessagesList.ChangePasswordSuccess.GetMessage().Code,
            MessagesList.ChangePasswordSuccess.GetMessage().Message
            ));
    }
}
