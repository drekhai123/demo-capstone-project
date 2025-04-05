using EduSource.Domain.Abstraction.Dappers.Repositories;
using EduSource.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace EduSource.Infrastructure.Dapper.Repositories;

public class ProductInComboRepository : IProductInComboRepository
{
    private readonly IConfiguration _configuration;
    public ProductInComboRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<int> AddAsync(ProductInCombo entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(ProductInCombo entity)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<ProductInCombo>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ProductInCombo>? GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(ProductInCombo entity)
    {
        throw new NotImplementedException();
    }
}
