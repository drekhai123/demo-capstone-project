using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace EduSource.Infrastructure.Dapper.Repositories;

public class CartRepository : ICartRepository
{
    private readonly IConfiguration _configuration;
    public CartRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<int> AddAsync(Cart entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(Cart entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteByAccountId(Guid AccountId)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Cart>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Cart>? GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(Cart entity)
    {
        throw new NotImplementedException();
    }
}
