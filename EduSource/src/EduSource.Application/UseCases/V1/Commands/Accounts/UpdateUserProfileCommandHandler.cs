using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Accounts;
using EduSource.Domain.Abstraction.EntitiyFramework;
using static EduSource.Domain.Exceptions.AccountException;

namespace EduSource.Application.UseCases.V1.Commands.Account;

public sealed class UpdateInfoProfileCommandHandler : ICommandHandler<Command.UpdateInfoCommand, Success<Response.UserResponse>>
{
    private readonly IEFUnitOfWork _efUnitOfWork;

    public UpdateInfoProfileCommandHandler(IEFUnitOfWork efUnitOfWork)
    {
        _efUnitOfWork = efUnitOfWork;
    }

    public async Task<Result<Success<Response.UserResponse>>> Handle(Command.UpdateInfoCommand request, CancellationToken cancellationToken)
    {
        var result = await _efUnitOfWork.AccountRepository.FindByIdAsync(request.UserId);
        if (result == null)
            throw new AccountNotFoundException();
        
        result.UpdateInfoProfileUser(request.FirstName, request.LastName, request.PhoneNumber, request.Gender);
        _efUnitOfWork.AccountRepository.Update(result);
        await _efUnitOfWork.SaveChangesAsync(cancellationToken);

        var response = new Response.UserResponse(result.Id, result.FirstName, result.LastName, result.Email, result.PhoneNumber, result.GenderType);
        
        return Result.Success(new Success<Response.UserResponse>
            (MessagesList.AccountUpdateInformationSuccess.GetMessage().Code,
            MessagesList.AccountUpdateInformationSuccess.GetMessage().Message,
            response));
    }
}
