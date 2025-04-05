using EduSource.Contract.Abstractions.Message;
using EduSource.Contract.Abstractions.Shared;
using static EduSource.Contract.Services.Accounts.Response;

namespace EduSource.Contract.Services.Accounts
{
    public static class Query
    {
        public record GetUserProfileQuery(Guid UserId) : IQuery<Success<UserInfoResponse>>;

        public record GetUsersQueryHandler(int PageIndex,
         int PageSize,
         Filter.AccountsFilter FilterParams,
         string[] SelectedColumns)
         : IQuery<Success<PagedResult<UsersResponse>>>;
    }
}
