using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Enumarations.MessagesList;
using EduSource.Contract.Services.Accounts;
using EduSource.Domain.Abstraction.Dappers;
using static EduSource.Domain.Exceptions.AccountException;

namespace EduSource.Application.UseCases.V1.Queries.User;

public sealed class GetUserProfileQueryHandler : IQueryHandler<Query.GetUserProfileQuery, Success<Response.UserInfoResponse>>
{
    private readonly IDPUnitOfWork _dpUnitOfWork;

    public GetUserProfileQueryHandler(IDPUnitOfWork dpUnitOfWork)
    {
        _dpUnitOfWork = dpUnitOfWork;
    }

    public async Task<Result<Success<Response.UserInfoResponse>>> Handle(Query.GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var selectedColumn = new[] { "Id", "LoginType", "FirstName", "LastName", "Email", "PhoneNumber", "Status", "RoleId", "GenderType", "IsDeleted", "CropAvatarUrl", "FullAvatarUrl", "CropCoverPhotoUrl", "FullCoverPhotoUrl", "Biography" };
        var result = await _dpUnitOfWork.AccountRepositories.GetPagedAsync(1, 1, new Filter.AccountFilter(request.UserId, "", false, Contract.Enumarations.Authentication.RoleType.Member), selectedColumn);
        if (result.Items?.Count() == 0) {
            throw new AccountNotFoundException();
        }
        var userResponse = new Response.UserInfoResponse
            (result.Items[0].Id, result.Items[0].FirstName, result.Items[0].LastName, result.Items[0].Email, result.Items[0].PhoneNumber, result.Items[0].GenderType, result.Items[0].LoginType, result.Items[0].CropAvatarUrl, result.Items[0].FullAvatarUrl, result.Items[0].CropCoverPhotoUrl, result.Items[0].FullCoverPhotoUrl, result.Items[0].Biography);
        return Result.Success(new Success<Response.UserInfoResponse>(MessagesList.AccountGetInfoProfileSuccess.GetMessage().Code, MessagesList.AccountGetInfoProfileSuccess.GetMessage().Message, userResponse));
    }
}
