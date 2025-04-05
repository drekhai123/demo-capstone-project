using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Abstraction.EntitiyFramework.Repositories;
using EduSource.Domain.Entities;

namespace EduSource.Domain.Abstraction.Dappers.Repositories;

public interface ICartRepository : IGenericRepository<Cart>
{
    Task<int> DeleteByAccountId(Guid AccountId);
}
