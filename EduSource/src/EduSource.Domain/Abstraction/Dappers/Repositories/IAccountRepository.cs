using EduSource.Contract.Abstractions.Shared;
using EduSource.Contract.Services.Accounts;
using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Entities;

namespace EduSource.Domain.Abstraction.Dappers.Repositories;

public interface IAccountRepository : IGenericRepository<Account>
{
    Task<bool> EmailExistSystemAsync(string email);
    Task<bool>? AccountExistSystemAsync(Guid userId);
    Task<Account> GetByEmailAsync(string email);
    Task<int> CountAllUsers();
    Task<PagedResult<Account>> GetPagedAsync(int pageIndex, int pageSize, Filter.AccountFilter filterParams, string[] selectedColumns);
}
