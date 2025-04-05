using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;

namespace EduSource.Domain.Abstraction.EntitiyFramework.Repositories;

public interface IAccountRepository : IRepositoryBase<Account, Guid>
{
    Task<Account> GetAccountByEmailAsync(string email);
}
