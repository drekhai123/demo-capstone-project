using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace EduSource.Infrastructure.Dapper.Repositories;

public class ImageOfProductRepository : IImageOfProductRepository
{
    private readonly IConfiguration _configuration;
    public ImageOfProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<int> AddAsync(ImageOfProduct entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(ImageOfProduct entity)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<ImageOfProduct>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ImageOfProduct>? GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(ImageOfProduct entity)
    {
        throw new NotImplementedException();
    }
}
