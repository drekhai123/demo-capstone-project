using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace EduSource.Infrastructure.Dapper.Repositories;

public class WishlistRepository : IWishlistRepository
{
    private readonly IConfiguration _configuration;
    public WishlistRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<int> AddAsync(Wishlist entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(Wishlist entity)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Wishlist>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Wishlist>? GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(Wishlist entity)
    {
        throw new NotImplementedException();
    }
}
